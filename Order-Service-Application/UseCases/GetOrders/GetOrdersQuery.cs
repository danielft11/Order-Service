using MediatR;
using Order_Service_Application.DTOs;

namespace Order_Service_Application.UseCases.GetOrders
{
    public record GetOrdersQuery() : IRequest<GetOrdersResponseDTO>;
    
}
