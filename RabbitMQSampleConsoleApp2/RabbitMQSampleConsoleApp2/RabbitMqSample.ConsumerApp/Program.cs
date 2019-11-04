using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqSample.Core;
using System;
using System.Text;

namespace RabbitMqSample.ConsumerApp
{
    internal class Program
    {
        private static string queueName = "mustiscl";

        private static void Main(string[] args)
        {
            Consumer consumer = new Consumer(queueName);

            Console.ReadLine();
        }

        public static void Consume(object obj, BasicDeliverEventArgs evt)
        {
            var body = evt.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine("\"{0}\" queue üzerine, \"{1}\" mesajı geldi.", queueName, message);
        }
    }

    internal class Consumer
    {
        private readonly RabbitMqService rabbitMqservice;
        private string queueName = "mustiscl";
        private volatile bool autoAck = false;
        private const string exchangeName = "HelloWorld_RabbitMQ";

        public Consumer(string queueName)
        {
            this.queueName = queueName;

            rabbitMqservice = new RabbitMqService();

            using (var connection = rabbitMqservice.GetConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(this.queueName, false, false, false, null);
                    var consumer = new EventingBasicConsumer(channel);
                    while (true)
                    {
                        BasicGetResult result = channel.BasicGet(this.queueName, true);
                        if (result != null)
                        {
                            string data =
                            Encoding.UTF8.GetString(result.Body);
                            Console.WriteLine(data);
                        }
                    }
                    //channel.QueueDeclare(queue: this.queueName,
                    //                     durable: false,
                    //                     exclusive: false,
                    //                     autoDelete: false,
                    //                     arguments: null);

                    //var consumer = new EventingBasicConsumer(channel);
                    //channel.BasicConsume(
                    //    queue: queueName,
                    //                 autoAck: false,
                    //                 consumer: consumer);

                    //consumer.Received += Program.Consume;
                    /*(model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("\"{0}\" queue üzerine, \"{1}\" mesajı geldi.", queueName, message);
                };
                */
                    //var receiver = new MessageReceiver(channel);
                    //channel.BasicConsume(queue: queueName,
                    //                     autoAck: true,
                    //                     consumer: receiver);
                }
            }
        }

        private void Consume(object obj, BasicDeliverEventArgs evt)
        {
            var body = evt.Body;
            var message = Encoding.UTF8.GetString(body);
            autoAck = !string.IsNullOrWhiteSpace(message);
            Console.WriteLine("\"{0}\" queue üzerine, \"{1}\" mesajı geldi.", queueName, message);
        }
    }

    public class MessageReceiver : DefaultBasicConsumer
    {
        private readonly IModel _channel;

        public MessageReceiver(IModel channel)
        {
            _channel = channel;
        }

        public override void HandleBasicDeliver(
            string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            Console.WriteLine($"Consuming Message");

            Console.WriteLine(string.Concat("Message received from the exchange ", exchange));

            Console.WriteLine(string.Concat("Consumer tag: ", consumerTag));

            Console.WriteLine(string.Concat("Delivery tag: ", deliveryTag));

            Console.WriteLine(string.Concat("Routing tag: ", routingKey));

            Console.WriteLine(string.Concat("Message: ", Encoding.UTF8.GetString(body)));

            _channel.BasicAck(deliveryTag, false);
        }
    }
}