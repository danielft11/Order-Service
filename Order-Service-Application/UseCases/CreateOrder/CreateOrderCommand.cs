using MediatR;
using Order_Service_Application.DTOs;

namespace Order_Service_Application.UseCases.CreateOrder
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public string UserId { get; set; }
        public List<CreateOrderItemDTO> Items { get; set; }
    }
}
