using Order_Service_Application.DTOs;
using Order_Service_Domain.Entities;
using Order_Service_Domain.Enums;

namespace Order_Service_Application.UseCases.GetOrders.Mappings
{
    public static class GetOrdersMapping
    {
        public static IEnumerable<GetOrderDTO> MapOrders(this IEnumerable<Order> orders)
        {
            return orders.Select(order => new GetOrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                Status = MapStatus(order.Status),
                CreatedAt = order.CreatedAt,
                Items = order.Items.MapItems()
            });
        }

        private static string MapStatus(OrderStatusEnum status) => status switch
        {
            OrderStatusEnum.Pending => "Pending",
            OrderStatusEnum.AwaitingStock => "Await Stock",
            OrderStatusEnum.StockUpdated => "Stock Updated",
            OrderStatusEnum.AwaitingPayment => "Awaiting Payment",
            OrderStatusEnum.Completed => "Completed",
            OrderStatusEnum.Cancelled => "Cancelled",
            _ => ""
        };

        private static IEnumerable<GetOrderItemDTO> MapItems(this List<OrderItem> items)
        {
            return items.Select(item => new GetOrderItemDTO
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                UnitPrice = item.Price,
                Quantity = item.Quantity,
                TotalValue = item.Price * item.Quantity
            });
        }

    }
}
