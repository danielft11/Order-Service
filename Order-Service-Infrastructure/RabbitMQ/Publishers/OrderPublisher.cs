using RabbitMQ.Client;

namespace Order_Service_Infrastructure.RabbitMQ.Publishers
{
    public class OrderPublisher() : IEventPublisher
    {
        private readonly ConnectionFactory _factory = new()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        public Task PublishAsync(byte[] messageBytes)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: "order.events",
                type: ExchangeType.Topic,
                durable: true);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(
                exchange: "order.events",
                routingKey: "order.created",
                basicProperties: properties,
                body: messageBytes);

            return Task.CompletedTask;
        }

    }

    public interface IEventPublisher
    {
        Task PublishAsync(byte[] messageBytes);
    }
}
