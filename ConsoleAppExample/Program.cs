using RabbitMQ.Client;

namespace ConsoleAppExample
{
    internal class Program
    {
        public const string _HostName = "localhost";
        public const string _UserName = "guest";
        public const string _Password = "guest";

        public const string _FirstQueueName = "Module2.Sample4.Queue1";
        public const string _SecondQueueName = "Module2.Sample4.Queue2";
        public const string _ExchangeName = "Module2.Sample4.Exchange";

        static void Main(string[] args)
        {
            Console.WriteLine("Setting up RabbitMQ Connection Factory");
            var factory = new ConnectionFactory
            {
                HostName = _HostName,
                UserName = _UserName,
                Password = _Password
            };

            using var connection = factory.CreateConnection();

            Console.WriteLine("Setting up RabbitMQ Channel");
            using var channel = connection.CreateModel();

            Console.WriteLine("Creating Exchange");

            channel.ExchangeDeclare(
                exchange: _ExchangeName,
                // Direct, is used to route messages with a specific routing key.
                type: ExchangeType.Direct,
                durable: true);

            Console.WriteLine("Creating Server 1 Queue");
            channel.QueueDeclare(
                queue: _FirstQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(_FirstQueueName, _ExchangeName, "1");

            Console.WriteLine("Creating Server 2 Queue");
            channel.QueueDeclare(
                queue: _SecondQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(_SecondQueueName, _ExchangeName, "2");

            Console.WriteLine("Setup complete");
        }    
    }
}


