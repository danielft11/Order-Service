namespace Order_Service_Application.Outbox
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public string UserId { get; set; }
        public decimal Total { get; set; }
        public List<OrderItemEvent> Items { get; set; }
    }

    public class OrderItemEvent 
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
