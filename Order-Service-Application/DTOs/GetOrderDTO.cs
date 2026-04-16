using Order_Service_Domain.Enums;

namespace Order_Service_Application.DTOs
{
    public class GetOrderDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<GetOrderItemDTO> Items { get; set; }
    }
}
