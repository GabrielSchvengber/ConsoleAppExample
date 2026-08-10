using RabbitMQ.Client;

namespace ConsoleAppExample
{
    internal class Program
    {
        public const string _HostName = "localhost";
        public const string _UserName = "guest";
        public const string _Password = "guest";

        public const string _QueueName = "Module2.Sample7.Queue";

        static void Main(string[] args)
        {
            string rabbitDllPath = args.Length > 0 ? args[0] : "not specified";

            Console.WriteLine("Rabbit DLL Path: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(rabbitDllPath);
            Console.ResetColor();

            Console.WriteLine("Setting up RabbitMQ Connection Factory");
            Console.ForegroundColor = ConsoleColor.Green;

            var connectionFactory = new ConnectionFactory
            {
                HostName = _HostName,
                UserName = _UserName,
                Password = _Password
            };

            var connection = connectionFactory.CreateConnection();

            Console.WriteLine("Setting up RabbitMQ Channel");
            var channel = connection.CreateModel();

            Console.WriteLine("Creating RPC Queue");
            channel.QueueDeclare(
                queue: _QueueName,                
                durable: false,
                exclusive: true,

                autoDelete: false,
                arguments: null);

            Console.ResetColor();
            Console.WriteLine("Setup complete");
        }
    }
}


