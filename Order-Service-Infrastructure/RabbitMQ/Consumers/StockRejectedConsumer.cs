using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Order_Service_Application.Events;
using Order_Service_Application.UseCases.CreateOrder.Commands;
using Order_Service_Domain.Enums;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Order_Service_Infrastructure.RabbitMQ.Consumers
{
    public class StockRejectedConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IModel _channel;
        private readonly IRabbitMQConnection _rabbitMQConnection;

        private static readonly string INVENTORY_EXCHANGE = "inventory.exchange";
        private static readonly string STOCK_REJECTED_QUEUE = "stock.rejected.queue";
        private static readonly string STOCK_REJECTED_ROUTING_KEY = "stock.rejected";

        public StockRejectedConsumer(IServiceProvider serviceProvider, IRabbitMQConnection rabbitMQConnection)
        {
            _rabbitMQConnection = rabbitMQConnection;
            _serviceProvider = serviceProvider;

            var _connection = _rabbitMQConnection.GetConnection();
            _channel = _connection.CreateModel();
            _channel.BasicQos(0, 10, false);

            _channel.ExchangeDeclare(INVENTORY_EXCHANGE, ExchangeType.Topic, durable: true);
            _channel.QueueDeclare(queue: STOCK_REJECTED_QUEUE, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(STOCK_REJECTED_QUEUE, INVENTORY_EXCHANGE, STOCK_REJECTED_ROUTING_KEY);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
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

                    await mediator.Send(new UpdateOrderStatusCommand
                    {
                        MessageId = message.MessageId,
                        OrderId = message.OrderId,
                        OrderStatus = OrderStatusEnum.Cancelled
                    });

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
                queue: STOCK_REJECTED_QUEUE,
                autoAck: false,
                consumerTag: "Stock-Rejected-Consumer",
                consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
