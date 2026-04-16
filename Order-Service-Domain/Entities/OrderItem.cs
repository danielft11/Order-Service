namespace Order_Service_Domain.Entities
{
    public sealed class OrderItem
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }

        public string ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }

        protected OrderItem() { }

        public OrderItem(string productId, string productName, decimal price, int quantity)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
        }
    }
}
