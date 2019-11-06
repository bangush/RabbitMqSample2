namespace RabbitMqSample.Core
{
    using RabbitMQ.Client;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="Publisher" />
    /// </summary>
    public class Publisher
    {
        /// <summary>
        /// Defines the rabbitMqservice
        /// </summary>
        private readonly RabbitMqService rabbitMqservice;

        /// <summary>
        /// Defines the queueName
        /// </summary>
        private readonly string queueName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Publisher"/> class.
        /// </summary>
        /// <param name="queueName">The queueName<see cref="string"/></param>
        public Publisher(string queueName)
        {
            rabbitMqservice = new RabbitMqService();
            this.queueName = queueName;
        }

        /// <summary>
        /// The Publish
        /// </summary>
        /// <param name="message">The message<see cref="string"/></param>
        public void Publish(string message)
        {
            using (var connection = rabbitMqservice.GetConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: this.queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null
                        );
                    channel.BasicPublish(exchange: "", routingKey: this.queueName, basicProperties: null, body: Encoding.UTF8.GetBytes(message));
                }
            }
        }

        public void Publish(byte[] message)
        {
            using (var connection = rabbitMqservice.GetConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: this.queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null
                        );
                    channel.BasicPublish(exchange: "", routingKey: this.queueName, basicProperties: null, body: message);
                }
            }
        }
    }
}
