using RabbitMQ.Client;
namespace ConsoleAppExample
{
    internal class Program
    {
        public const string _HostName = "localhost";
        public const string _UserName = "guest";
        public const string _Password = "guest";

        public const string _FirstQueueName = "Module2.Sample5.Queue1";
        public const string _SecondQueueName = "Module2.Sample5.Queue2";
        public const string _ThirdQueueName = "Module2.Sample5.Queue3";

        public const string _ExchangeName = "Module2.Sample5.Exchange";

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
                // Topic, is used to route messages based on pattern matching with wildcards (* and #).
                type: ExchangeType.Topic,
                durable: true);

            Console.WriteLine("Creating Server 1 Queue");
            channel.QueueDeclare(
                queue: _FirstQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(_FirstQueueName, _ExchangeName, "*.high.*");

            Console.WriteLine("Creating Server 2 Queue");
            channel.QueueDeclare(
                queue: _SecondQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(_SecondQueueName, _ExchangeName, "*.*.cupboard");

            Console.WriteLine("Creating Server 3 Queue");
            channel.QueueDeclare(
                queue: _ThirdQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(_ThirdQueueName, _ExchangeName, "*.medium.*");
            channel.QueueBind(_ThirdQueueName, _ExchangeName, "corporate.#");

            Console.WriteLine("Setup complete");
        }
    }
}