namespace Order_Service_Application.DTOs
{
    public class GetOrderItemDTO
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalValue { get; set; }
    }
}
