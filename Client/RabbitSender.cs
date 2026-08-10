using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace Client
{
    public class RabbitSender : IDisposable
    {
        private const string _HostName = "localhost";
        private const string _UserName = "guest";
        private const string _Password = "guest";
        private const string _QueueName = "Module2.Sample7.Queue";
        private const bool _IsDurable = true;

        private const string _VirtualHost = "";
        private int _Port = 0;

        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _model;
        private bool _disposed;

        private string _responseQueue;
        private EventingBasicConsumer _consumer;

        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _pendingRequests =
            new ConcurrentDictionary<string, TaskCompletionSource<string>>();

        /// <summary>
        /// Ctor
        /// </summary>
        public RabbitSender()
        {
            DisplaySettings();
            SetupRabbitMq();
        }

        private void DisplaySettings()
        {
            Console.WriteLine("Host: {0}", _HostName);
            Console.WriteLine("Username: {0}", _UserName);
            Console.WriteLine("Password: {0}", _Password);
            Console.WriteLine("QueueName: {0}", _QueueName);
            Console.WriteLine("VirtualHost: {0}", _VirtualHost);
            Console.WriteLine("Port: {0}", _Port);
            Console.WriteLine("Is Durable: {0}", _IsDurable);
        }

        private void SetupRabbitMq()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = _HostName,
                UserName = _UserName,
                Password = _Password
            };

            if (string.IsNullOrEmpty(_VirtualHost) == false)
                _connectionFactory.VirtualHost = _VirtualHost;
            if (_Port > 0)
                _connectionFactory.Port = _Port;

            _connection = _connectionFactory.CreateConnection();
            _model = _connection.CreateModel();

            // Create dynamic response queue
            _responseQueue = _model.QueueDeclare().QueueName;

            _consumer = new EventingBasicConsumer(_model);
            _consumer.Received += OnMessageReceived;

            _model.BasicConsume(_responseQueue, true, _consumer);
        }

        private void OnMessageReceived(object? sender, BasicDeliverEventArgs e)
        {
            var correlationId = e.BasicProperties?.CorrelationId;
            if (correlationId == null)
                return;

            if (_pendingRequests.TryRemove(correlationId, out var tcs))
            {
                var response = Encoding.Default.GetString(e.Body.Span);
                tcs.TrySetResult(response);
            }
        }

        public string Send(string message, TimeSpan timeout)
        {
            var correlationToken = Guid.NewGuid().ToString();

            var tcs = new TaskCompletionSource<string>();
            _pendingRequests[correlationToken] = tcs;

            try
            {
                var properties = _model.CreateBasicProperties();
                properties.ReplyTo = _responseQueue;
                properties.CorrelationId = correlationToken;

                byte[] messageBuffer = Encoding.Default.GetBytes(message);
                _model.BasicPublish("", _QueueName, properties, messageBuffer);

                if (tcs.Task.Wait(timeout))
                    return tcs.Task.Result;

                throw new TimeoutException("The response was not returned before the timeout");
            }
            finally
            {
                _pendingRequests.TryRemove(correlationToken, out _);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_consumer != null)
                    _consumer.Received -= OnMessageReceived;
                
                foreach (var pending in _pendingRequests)
                {
                    pending.Value.TrySetCanceled();
                }
                _pendingRequests.Clear();

                try
                {
                    if (_model?.IsOpen == true)
                        _model.Close();
                }
                catch (Exception)
                {
                    Console.WriteLine("Error Disposing the _model.");
                }
                finally
                {
                    _model?.Dispose();
                }

                try
                {
                    if (_connection?.IsOpen == true)
                        _connection.Close();
                }
                catch (Exception)
                {
                    Console.WriteLine("Error Disposing the _connection.");
                }
                finally
                {
                    _connection?.Dispose();
                }
            }

            _disposed = true;
        }

    }
}
