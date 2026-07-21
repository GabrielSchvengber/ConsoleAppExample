using System;

namespace ConsoleAppExample
{
    internal class Program
    {
        public const string _HostName = "localhost";
        public const string _UserName = "guest";
        public const string _Password = "guest";

        private static void Main(string[] args)
        {
            var connectionFactory = new RabbitMQ.Client.ConnectionFactory(){
                HostName = _HostName,
                UserName = _UserName,
                Password = _Password
            };

            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);
            //Many Variations up here on channel,
            //but the main thing is to create a channel and then use it to send and receive messages.

        }
    }
}


