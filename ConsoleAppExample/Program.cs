using RabbitMQ.Client;

namespace ConsoleAppExample
{
    internal class Program
    {
        public const string _HostName = "localhost";
        public const string _UserName = "guest";
        public const string _Password = "guest";

        public const string _FirstQueueName = "Module2.Sample6.Queue1";
        public const string _SecondQueueName = "Module2.Sample6.Queue2";
        public const string _ThirdQueueName = "Module2.Sample6.Queue3";
        public const string _FourthQueueName = "Module2.Sample6.Queue4";

        public const string _ExchangeName = "Module2.Sample6.Exchange";

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
                // Headers, it's use to route messages based on attributes (headers).
                type: ExchangeType.Headers,
                durable: true);

            Console.WriteLine("Creating Server 1 Queue");

            channel.QueueDeclare(
                queue: _FirstQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var header1 = new Dictionary<string, object>
            {
                { "material", "wood" },
                { "customertype", "b2b" }
            };

            channel.QueueBind(_FirstQueueName, _ExchangeName, "", header1);

            Console.WriteLine("Creating Server 2 Queue");

            channel.QueueDeclare(
                queue: _SecondQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var header2 = new Dictionary<string, object>
            {
                { "material", "metal" },
                { "customertype", "b2c" }
            };

            channel.QueueBind(_SecondQueueName, _ExchangeName, "", header2);

            Console.WriteLine("Creating Server 3 Queue");

            channel.QueueDeclare(
                queue: _ThirdQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var header3 = new Dictionary<string, object>
            {
                { "x-match", "any" },
                { "material", "wood" },
                { "customertype", "b2b" }
            };

            channel.QueueBind(_ThirdQueueName, _ExchangeName, "", header3);

            Console.WriteLine("Creating Server 4 Queue");

            channel.QueueDeclare(
                queue: _FourthQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var header4 = new Dictionary<string, object>
            {
                { "x-match", "any" },
                { "material", "metal" },
                { "customertype", "b2c" }
            };

            channel.QueueBind(_FourthQueueName, _ExchangeName, "", header4);

            Console.WriteLine("Setup complete");
        }
    }
}