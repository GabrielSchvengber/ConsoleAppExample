using RabbitMQ.Client;
using System.Text;

namespace Client
{
    public class RabbitSender : IDisposable
    {
        private const string _HostName = "localhost";
        private const string _UserName = "guest";
        private const string _Password = "guest";
        private const string _QueueName = "Module2.Sample2";
        private const string _ExchangeName = "";
        private const bool _IsDurable = true;

        //The two below settings are just default values.
        private const string _VirtualHost = "";
        private int _Port = 0;

        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _model;
        private bool _disposed;

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
        }

        public void Send(string message)
        {
            var properties = _model.CreateBasicProperties();
            properties.Persistent = true;

            byte[] messageBuffer = Encoding.Default.GetBytes(message);

            _model.BasicPublish(_ExchangeName, _QueueName, properties, messageBuffer);
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
