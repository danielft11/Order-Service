using Order_Service_Domain.Enums;

namespace Order_Service_Domain.Entities
{
    public sealed class Order
    {
        public Guid Id { get; private set; }
        public string UserId { get; private set; }
        public decimal TotalAmount { get; private set; }
        public OrderStatusEnum Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public List<OrderItem> Items { get; private set; }

        protected Order() { }

        public Order(string userId, List<OrderItem> items)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Items = items;
            TotalAmount = items.Sum(i => i.Price * i.Quantity);
            Status = OrderStatusEnum.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetOrderStatus(OrderStatusEnum newStatus) => Status = newStatus;

        public void ReservedStock() => Status = OrderStatusEnum.StockUpdated;

        public void MarkAsPaid() => Status = OrderStatusEnum.Completed;

        public void Cancel() => Status = OrderStatusEnum.Cancelled;

    }
}
