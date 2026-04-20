using Contracts;
using MassTransit;
using MediatR;
using Order_Service_Application.UseCases.UpdateOrder;
using Order_Service_Domain.Enums;

namespace Order_Service_Infrastructure.RabbitMQ.Consumers
{
    public class StockRejectedConsumer(IMediator mediator) : IConsumer<StockReservedEvent>
    {
        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var message = context.Message;

            await mediator.Send(new UpdateOrderStatusCommand(message.MessageId, message.OrderId, OrderStatusEnum.Cancelled));
        }

    }
}
