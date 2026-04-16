using MediatR;
using Order_Service_Domain.Enums;

namespace Order_Service_Application.UseCases.UpdateOrder
{
    public record UpdateOrderStatusCommand(Guid MessageId, Guid OrderId, OrderStatusEnum OrderStatus) : IRequest<Unit>;
}
