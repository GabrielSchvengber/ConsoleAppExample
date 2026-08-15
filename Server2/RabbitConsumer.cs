using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Server2
{
    public class RabbitConsumer : IDisposable
    {
        private const string _HostName = "localhost";
        private const string _UserName = "guest";
        private const string _Password = "guest";

        private const string _QueueName = "Module2.Sample8.Queue2";
        private const bool _IsDurable = true;

        private const string _VirtualHost = "";
        private int _Port = 0;

        public delegate void OnReceiveMessage(string message);

        public bool Enabled { get; set; }

        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _model;
        private EventingBasicConsumer _consumer;

        public RabbitConsumer()
        {
            DisplaySettings();
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
            _model.BasicQos(0, 1, false);
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

        public void Start()
        {
            Enabled = true;

            _consumer = new EventingBasicConsumer(_model);
            _consumer.Received += OnMessageReceived;

            _model.BasicConsume(_QueueName, autoAck: false, consumer: _consumer);
        }

        private void OnMessageReceived(object? sender, BasicDeliverEventArgs deliveryArgs)
        {
            if (!Enabled)
                return;

            var message = Encoding.Default.GetString(deliveryArgs.Body.ToArray());
            Console.WriteLine("Received message: {0}", message);

            var response = ProcessMessage(message);

            SendReply(deliveryArgs, response);

            Console.WriteLine("Finished Processing Message - {0}", message);

            _model.BasicAck(deliveryArgs.DeliveryTag, multiple: false);
        }

        private string ProcessMessage(string message)
        {
            return string.Format("Processed message - {0} : Response is good", message);
        }

        private void SendReply(BasicDeliverEventArgs deliveryArgs, string response)
        {
            var replyTo = deliveryArgs.BasicProperties?.ReplyTo;
            if (string.IsNullOrEmpty(replyTo))
                return;

            var replyProperties = _model.CreateBasicProperties();
            replyProperties.CorrelationId = deliveryArgs.BasicProperties?.CorrelationId;

            byte[] messageBuffer = Encoding.Default.GetBytes(response);

            _model.BasicPublish(exchange: "", routingKey: replyTo, basicProperties: replyProperties, body: messageBuffer);
        }

        public void Dispose()
        {
            if (_consumer != null)
                _consumer.Received -= OnMessageReceived;
            if (_model != null)
                _model.Dispose();
            if (_connection != null)
                _connection.Dispose();

            _connectionFactory = null;

            GC.SuppressFinalize(this);
        }
    }
}