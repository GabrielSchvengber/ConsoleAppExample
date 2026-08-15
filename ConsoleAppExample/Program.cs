using RabbitMQ.Client;

namespace ConsoleAppExample
{
    internal class Program
    {
        public const string _HostName = "localhost";
        public const string _UserName = "guest";
        public const string _Password = "guest";

        public const string _FirstQueueName = "Module2.Sample8.Queue1";
        public const string _SecondQueueName = "Module2.Sample8.Queue2";
        public const string _ThirdQueueName = "Module2.Sample8.Queue3";

        public const string _ExchangeName = "Module2.Sample8.Exchange";

        static void Main(string[] args)
        {
            Console.WriteLine("Setting up RabbitMQ Connection Factory");

            var factory = new ConnectionFactory
            {
                HostName = _HostName,
                UserName = _UserName,
                Password = _Password
            };

            var connection = factory.CreateConnection();

            Console.WriteLine("Setting up RabbitMQ Channel");

            var channel = connection.CreateModel();

            Console.WriteLine("Creating Exchange");

            channel.ExchangeDeclare(
                exchange: _ExchangeName,
                // Topic, It1s used to route messages based on a pattern matching between the routing key and the binding key.
                type: ExchangeType.Topic,
                durable: false);

            Console.WriteLine("Creating Server 1 Queue");

            channel.QueueDeclare(
                queue: _FirstQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.QueueBind(_FirstQueueName, _ExchangeName, "1");
            channel.QueueBind(_FirstQueueName, _ExchangeName, "4");

            Console.WriteLine("Creating Server 2 Queue");

            channel.QueueDeclare(
                queue: _SecondQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.QueueBind(_SecondQueueName, _ExchangeName, "2");
            channel.QueueBind(_SecondQueueName, _ExchangeName, "4");
            channel.QueueBind(_SecondQueueName, _ExchangeName, "6");

            Console.WriteLine("Creating Server 3 Queue");

            channel.QueueDeclare(
                queue: _ThirdQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.QueueBind(_ThirdQueueName, _ExchangeName, "3");
            channel.QueueBind(_ThirdQueueName, _ExchangeName, "4");
            channel.QueueBind(_ThirdQueueName, _ExchangeName, "6");

            Console.WriteLine("Setup complete");
        }
    }
}