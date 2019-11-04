using RabbitMQ.Client;

namespace RabbitMqSample.Core
{
    public class RabbitMqService
    {
        private readonly string hostName = "localhost";

        public IConnection GetConnection()
        {
            IConnection connection = null;

            var factory = new ConnectionFactory();

            factory.HostName = hostName;

            connection = factory.CreateConnection();

            return connection;
        }
    }
}