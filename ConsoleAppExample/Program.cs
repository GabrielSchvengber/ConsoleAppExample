using RabbitMQ.Client;

namespace ConsoleAppExample
{
    internal class Program
    {
        public const string _HostName = "localhost";
        public const string _UserName = "guest";
        public const string _Password = "guest";

        public const string _FisrtQueueName = "Module2.Sample3.Queue1";
        public const string _SecondQueueName = "Module2.Sample3.Queue2";

        public const string _ExchangeName = "Module2.Sample3.Exchange";

        private static void Main(string[] args)
        {
            Console.WriteLine("Setting up RabbitMQ Connection Factory");

            var connectionFactory = new ConnectionFactory
            {
                HostName = _HostName,
                UserName = _UserName,
                Password = _Password
            };

            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            //A Fanout exchange sends the SAME message to all related queues.
            string exchangeType = ExchangeType.Fanout;

            channel.ExchangeDeclare(
                exchange: _ExchangeName,
                type: exchangeType,
                durable: true
            );

            Console.WriteLine("Creating Server 1 Queue");

            channel.QueueDeclare(
                queue: _FisrtQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            channel.QueueBind(
                queue: _FisrtQueueName,
                exchange: _ExchangeName,
                // Null routing key for fanout exchange, since it sends the same message to all queues.
                routingKey: ""
            );

            Console.WriteLine("Creating Server 2 Queue");

            channel.QueueDeclare(
                queue: _SecondQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            channel.QueueBind(
                queue: _SecondQueueName,
                exchange: _ExchangeName,
                // Null routing key for fanout exchange, since it sends the same message to all queues.
                routingKey: ""
            );

            Console.WriteLine("Setup complete");
        }
    }
}


