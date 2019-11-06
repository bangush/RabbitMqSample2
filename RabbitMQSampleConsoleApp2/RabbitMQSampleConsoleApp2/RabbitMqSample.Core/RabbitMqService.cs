namespace RabbitMqSample.Core
{
    using RabbitMQ.Client;

    /// <summary>
    /// Defines the <see cref="RabbitMqService" />
    /// </summary>
    public class RabbitMqService
    {
        /// <summary>
        /// Defines the hostName
        /// </summary>
        private readonly string hostName = "localhost";

        /// <summary>
        /// The GetConnection
        /// </summary>
        /// <returns>The <see cref="IConnection"/></returns>
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
