using RabbitMqSample.Core;
using System;

namespace RabbitMQSampleConsoleApp2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string queueName = "mustiscl";
            Publisher publisher = new Publisher(queueName);

            string data = string.Empty;
            Console.WriteLine("Write message to send.");
            Console.WriteLine("Write exit to exit.");

            do
            {
                data = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(data))
                {
                    publisher.Publish(data);
                }
            } while (data != "exit");

            Console.WriteLine("Enter to exit.");
            Console.ReadKey();
        }
    }
}