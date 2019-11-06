namespace RabbitMqSample.ConsumerApp
{
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using RabbitMqSample.Core;
    using System;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="Program" />
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Defines the queueName
        /// </summary>
        private static string queueName = "mustiscl";

        /// <summary>
        /// The Main
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/></param>
        private static void Main(string[] args)
        {
            Consumer consumer = new Consumer(queueName);

            Console.ReadLine();
        }

        /// <summary>
        /// The Consume
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/></param>
        /// <param name="evt">The evt<see cref="BasicDeliverEventArgs"/></param>
        public static void Consume(object obj, BasicDeliverEventArgs evt)
        {
            var body = evt.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine("\"{0}\" queue üzerine, \"{1}\" mesajı geldi.", queueName, message);
        }
    }

    /// <summary>
    /// Defines the <see cref="Consumer" />
    /// </summary>
    internal class Consumer
    {
        /// <summary>
        /// Defines the rabbitMqservice
        /// </summary>
        private readonly RabbitMqService rabbitMqservice;

        /// <summary>
        /// Defines the queueName
        /// </summary>
        private string queueName = "mustiscl";

        /// <summary>
        /// Defines the autoAck
        /// </summary>
        private volatile bool autoAck = false;

        /// <summary>
        /// Defines the exchangeName
        /// </summary>
        private const string exchangeName = "HelloWorld_RabbitMQ";

        /// <summary>
        /// Initializes a new instance of the <see cref="Consumer"/> class.
        /// </summary>
        /// <param name="queueName">The queueName<see cref="string"/></param>
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

        /// <summary>
        /// The Consume
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/></param>
        /// <param name="evt">The evt<see cref="BasicDeliverEventArgs"/></param>
        private void Consume(object obj, BasicDeliverEventArgs evt)
        {
            var body = evt.Body;
            var message = Encoding.UTF8.GetString(body);
            autoAck = !string.IsNullOrWhiteSpace(message);
            Console.WriteLine("\"{0}\" queue üzerine, \"{1}\" mesajı geldi.", queueName, message);
        }
    }

    /// <summary>
    /// Defines the <see cref="MessageReceiver" />
    /// </summary>
    public class MessageReceiver : DefaultBasicConsumer
    {
        /// <summary>
        /// Defines the _channel
        /// </summary>
        private readonly IModel _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageReceiver"/> class.
        /// </summary>
        /// <param name="channel">The channel<see cref="IModel"/></param>
        public MessageReceiver(IModel channel)
        {
            _channel = channel;
        }

        /// <summary>
        /// The HandleBasicDeliver
        /// </summary>
        /// <param name="consumerTag">The consumerTag<see cref="string"/></param>
        /// <param name="deliveryTag">The deliveryTag<see cref="ulong"/></param>
        /// <param name="redelivered">The redelivered<see cref="bool"/></param>
        /// <param name="exchange">The exchange<see cref="string"/></param>
        /// <param name="routingKey">The routingKey<see cref="string"/></param>
        /// <param name="properties">The properties<see cref="IBasicProperties"/></param>
        /// <param name="body">The body<see cref="byte[]"/></param>
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
