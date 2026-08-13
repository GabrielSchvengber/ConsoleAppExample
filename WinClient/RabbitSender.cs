using RabbitMQ.Client;
using System.Collections.Specialized;
using System.Text;

namespace WinClient
{
    public class RabbitSender : IDisposable
    {
        private const string _HostName = "localhost";
        private const string _UserName = "guest";
        private const string _Password = "guest";

        private const string _ExchangeName = "Module2.Sample6.Exchange";
        private const bool _IsDurable = true;
        
        private const string _VirtualHost = "";
        private int _Port = 0;

        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _model;

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
        /// <summary>
        /// Sets up the connections for rabbitMQ
        /// </summary>
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

        public void Send(string message, Dictionary<string, string> headers)
        {            
            var properties = _model.CreateBasicProperties();
            properties.Persistent = true;            
            properties.Headers = new Dictionary<string, object>();

            foreach (var header in headers)
            {
                Console.WriteLine("Header - Key: {0}, Value: {1}", header.Key, header.Value);
                properties.Headers.Add(header.Key, header.Value);
            }
          
            byte[] messageBuffer = Encoding.Default.GetBytes(message);
            
            _model.BasicPublish(_ExchangeName, "", properties, messageBuffer);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (_connection != null)
                _connection.Close();

            if (_model != null && _model.IsOpen)
                _model.Abort();

            _connectionFactory = null;

            GC.SuppressFinalize(this);
        }
    }
}
