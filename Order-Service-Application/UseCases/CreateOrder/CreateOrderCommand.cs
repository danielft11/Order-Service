using MediatR;
using Order_Service_Application.DTOs;

namespace Order_Service_Application.UseCases.CreateOrder
{
    public record CreateOrderCommand(string UserId, List<CreateOrderItemDTO> Items) : IRequest<Guid>;
}
