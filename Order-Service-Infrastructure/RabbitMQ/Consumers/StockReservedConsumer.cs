using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Order_Service_Application.Events;
using Order_Service_Application.UseCases.UpdateOrder;
using Order_Service_Domain.Enums;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Order_Service_Infrastructure.RabbitMQ.Consumers
{
    public class StockReservedConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IModel _channel;
        private readonly IRabbitMQConnection _rabbitMQConnection;

        private static readonly string INVENTORY_EXCHANGE = "inventory.exchange";
        private static readonly string STOCK_RESERVED_QUEUE = "stock.reserved.queue";
        private static readonly string STOCK_RESERVED_ROUTING_KEY = "stock.reserved";

        public StockReservedConsumer(IServiceProvider serviceProvider, IRabbitMQConnection rabbitMQConnection)
        {
            _rabbitMQConnection = rabbitMQConnection;
            _serviceProvider = serviceProvider;

            var _connection = _rabbitMQConnection.GetConnection(); 
            _channel = _connection.CreateModel();
            _channel.BasicQos(0, 10, false);

            _channel.ExchangeDeclare(INVENTORY_EXCHANGE, ExchangeType.Topic, durable: true);
            _channel.QueueDeclare(queue: STOCK_RESERVED_QUEUE, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(STOCK_RESERVED_QUEUE, INVENTORY_EXCHANGE, STOCK_RESERVED_ROUTING_KEY);
        }

        protected override async Task<Task> ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    var content = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                    var message = JsonConvert.DeserializeObject<StockReservedEvent>(content) ?? throw new JsonException("Mensagem inválida.");

                    var success = await mediator.Send(new UpdateOrderStatusCommand(message.MessageId, message.OrderId, OrderStatusEnum.StockUpdated));

                    _channel.BasicAck(eventArgs.DeliveryTag, false);

                }
                catch (JsonException ex)
                {
                    // Erro definitivo → requeue = false -> mensagem é descartada. Vai para o DLX e em seguida para a DLQ
                    _channel.BasicReject(eventArgs.DeliveryTag, requeue: false);
                }
                catch (Exception ex)
                {
                    // Erro transitório → requeue = true -> mensagem volta para a fila
                    _channel.BasicNack(eventArgs.DeliveryTag, false, requeue: true);
                }
            };

            _channel.BasicConsume(
                queue: STOCK_RESERVED_QUEUE,
                autoAck: false,
                consumerTag: "Stock-Reserved-Consumer",
                consumer: consumer);

            return Task.CompletedTask;

        }
    }
}
