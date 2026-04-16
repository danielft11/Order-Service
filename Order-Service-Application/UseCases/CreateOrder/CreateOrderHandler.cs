using MediatR;
using Order_Service_Application.DTOs;
using Order_Service_Application.Interfaces;
using Order_Service_Application.Outbox;
using Order_Service_Domain.Entities;
using Order_Service_Domain.Interfaces;
using System.Text.Json;

namespace Order_Service_Application.UseCases.CreateOrder
{
    public class CreateOrderHandler(IOrderRepository orderRepository, IOutboxRepository outboxRepository) : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IOutboxRepository _outboxRepository = outboxRepository;

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var items = MapItemsFromRequest(request.Items);

            var order = new Order(request.UserId, items);

            await _orderRepository.AddAsync(order);

            var @event = new OrderCreatedEvent
            {
                OrderId = order.Id,
                UserId = order.UserId,
                Total = order.TotalAmount,
                Items = order.Items.Select(i => new OrderItemEvent
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList()
            };

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = nameof(OrderCreatedEvent),
                Content = JsonSerializer.Serialize(@event),
                OccurredOn = DateTime.UtcNow
            };

            await _outboxRepository.AddAsync(outboxMessage);

            await _orderRepository.SaveChangesAsync();

            return order.Id;
        }

        private static List<OrderItem> MapItemsFromRequest(List<CreateOrderItemDTO> items)
        {
            return items
                .Select(i => new OrderItem(i.ProductId, i.ProductName, i.Price, i.Quantity))
                .ToList();
        }

    }
}
