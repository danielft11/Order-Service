using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Order_Service_Infrastructure.RabbitMQ
{
    public class RabbitMQConnection(IOptions<RabbitMqConfiguration> options) : IRabbitMQConnection
    {
        private readonly RabbitMqConfiguration _config = options.Value;

        public IConnection GetConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _config.Host,
                UserName = _config.UserName,
                Password = _config.Password
            };
            return factory.CreateConnection();
        }
    }

    public interface IRabbitMQConnection
    {
        IConnection GetConnection();
    }
}
