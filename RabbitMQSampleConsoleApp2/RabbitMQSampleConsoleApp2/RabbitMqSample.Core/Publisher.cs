using RabbitMQ.Client;
using System.Text;

namespace RabbitMqSample.Core
{
    public class Publisher
    {
        private readonly RabbitMqService rabbitMqservice;
        private readonly string queueName;

        public Publisher(string queueName)
        {
            rabbitMqservice = new RabbitMqService();
            this.queueName = queueName;
        }

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
    }
}