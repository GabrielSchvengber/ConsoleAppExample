using RabbitMQ.Client;

namespace ConsoleAppExample
{
    internal class Program
    {
        public const string _HostName = "localhost";
        public const string _UserName = "guest";
        public const string _Password = "guest";

        public const string _QueueName = "Module1.Sample3";
        public const string _ExchangeName = "MyExchange";

        private static void Main(string[] args)
        {
            Console.WriteLine("Starting RabbitMQ Message Sender");

            var connectionFactory = new ConnectionFactory()
            {
                HostName = _HostName,
                UserName = _UserName,
                Password = _Password
            };

            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: _ExchangeName,
                type: ExchangeType.Topic,
                durable: true);

            channel.QueueDeclare(
                queue: _QueueName,
                //Durable: true, // This will keep the queue even if RabbitMQ restarts.
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.QueueBind(
                queue: _QueueName,
                exchange: _ExchangeName,
                routingKey: _QueueName);


            var properties = channel.CreateBasicProperties();
            // Persistent = true, // Will keep the message in the queue even if RabbitMQ restarts.
            properties.Persistent = true;

            // messageBuffer.Length == 18
            // Each position in the array holds 1 byte, representing 1 piece of the text
            byte[] messageBuffer = System.Text.Encoding.UTF8.GetBytes("This is my Message");

            channel.BasicPublish(
                exchange: _ExchangeName,
                routingKey: _QueueName,
                basicProperties: properties,
                body: messageBuffer);

            Console.WriteLine("Message Sent to Queue: " + _QueueName);
            Console.ReadLine();
        }
    }
}


