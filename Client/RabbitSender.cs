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
        private const string _ExchangeName = "Module2.Sample8.Exchange";
        private const bool _IsDurable = true;

        private const string _VirtualHost = "";
        private int _Port = 0;

        private ConnectionFactory _connectionFactory;
        private EventingBasicConsumer _consumer;
        private IConnection _connection;
        private IModel _model;
        private bool _disposed;
        private string _responseQueue;
        
        private readonly ConcurrentDictionary<string, BlockingCollection<string>> _pendingRequests =
            new ConcurrentDictionary<string, BlockingCollection<string>>();

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
            Console.WriteLine("ExchangeName: {0}", _ExchangeName);
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

            _responseQueue = _model.QueueDeclare().QueueName;
            _consumer = new EventingBasicConsumer(_model);
            _consumer.Received += OnMessageReceived;

            _model.BasicConsume(_responseQueue, true, _consumer);
        }

        public QueueDeclareOk GetResponseQueueInfo()
        {
            return _model.QueueDeclarePassive(_responseQueue);
        }

        private void OnMessageReceived(object? sender, BasicDeliverEventArgs e)
        {
            var correlationId = e.BasicProperties?.CorrelationId;
            if (correlationId == null)
                return;

            if (_pendingRequests.TryGetValue(correlationId, out var responseCollection))
            {
                var response = Encoding.Default.GetString(e.Body.Span);
                responseCollection.Add(response);
            }
        }

        public List<string> Send(string message, string routingKey, TimeSpan timeout, int minResponses)
        {
            var correlationId = Guid.NewGuid().ToString();
            var responseBuffer = new BlockingCollection<string>();

            _pendingRequests[correlationId] = responseBuffer;

            try
            {
                PublishRequest(message, routingKey, correlationId);
                return CollectResponses(responseBuffer, timeout, minResponses);
            }
            finally
            {
                _pendingRequests.TryRemove(correlationId, out _);
                responseBuffer.Dispose();
            }
        }

        private void PublishRequest(string message, string routingKey, string correlationId)
        {
            var properties = _model.CreateBasicProperties();
            properties.ReplyTo = _responseQueue;
            properties.CorrelationId = correlationId;

            var body = Encoding.Default.GetBytes(message);
            _model.BasicPublish(_ExchangeName, routingKey, properties, body);
        }

        private static readonly TimeSpan _idleWindowAfterMinResponses = TimeSpan.FromMilliseconds(500);

        private static readonly TimeSpan _pollInterval = TimeSpan.FromMilliseconds(200);

        private List<string> CollectResponses(BlockingCollection<string> responseBuffer, TimeSpan timeout, int minResponses)
        {
            var responses = new List<string>();
            var deadline = DateTime.Now + timeout;

            while (DateTime.Now < deadline)
            {
                var timeLeft = deadline - DateTime.Now;
                var haveMinResponses = responses.Count >= minResponses;

                var chunk = haveMinResponses ? _idleWindowAfterMinResponses : _pollInterval;
                var waitTime = timeLeft < chunk ? timeLeft : chunk;

                if (!responseBuffer.TryTake(out var response, waitTime))
                {
                    if (haveMinResponses)
                        break; //if already have min responses, we can exit early if no more responses are coming in

                    Console.WriteLine("Waiting for responses");
                    continue; //if we haven't reached the minimum; if the time is up, the while loop will exit on its own
                }

                Console.WriteLine("Sender got response: {0}", response);
                responses.Add(response);
            }

            return responses;
        }

        public void Send(string message, string routingKey)
        {
            var properties = _model.CreateBasicProperties();
            properties.Persistent = true;

            byte[] messageBuffer = Encoding.Default.GetBytes(message);

            _model.BasicPublish(_ExchangeName, routingKey, properties, messageBuffer);
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