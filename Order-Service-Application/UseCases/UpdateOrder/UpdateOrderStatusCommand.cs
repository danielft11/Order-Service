using MediatR;
using Order_Service_Domain.Enums;

namespace Order_Service_Application.UseCases.UpdateOrder
{
    public class UpdateOrderStatusCommand : IRequest<Unit>
    {
        public Guid MessageId { get; set; }
        public Guid OrderId { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
    }
}
