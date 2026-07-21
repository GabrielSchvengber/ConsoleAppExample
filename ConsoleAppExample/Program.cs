using RabbitMQ.Client;

namespace ConsoleAppExample
{
    internal class Program
    {
        public const string _HostName = "localhost";
        public const string _UserName = "guest";
        public const string _Password = "guest";

        private static void Main(string[] args)
        {
            var connectionFactory = new ConnectionFactory(){
                HostName = _HostName,
                UserName = _UserName,
                Password = _Password
            };
            
            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();
           
            channel.QueueDeclare(queue: "MyQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            Console.WriteLine("Queue created");

            channel.ExchangeDeclare(exchange: "MyExchange", type: ExchangeType.Topic);
            Console.WriteLine("Exchange created");

            channel.QueueBind(queue: "MyQueue", exchange: "MyExchange", routingKey: "cars");
            Console.WriteLine("Queue bound to exchange with routing key 'cars'");

            Console.ReadLine();
        }
    }
}


