using MediatR;
using Order_Service_Application.DTOs;
using Order_Service_Application.UseCases.GetOrders.Mappings;
using Order_Service_Domain.Interfaces;

namespace Order_Service_Application.UseCases.GetOrders
{
    public class GetOrdersHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrdersQuery, GetOrdersResponseDTO>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<GetOrdersResponseDTO> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetOrders();

            return orders is null ? new GetOrdersResponseDTO([]) : new GetOrdersResponseDTO(orders.MapOrders());
        }

    }
}


