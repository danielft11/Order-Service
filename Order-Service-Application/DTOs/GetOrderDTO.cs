using Order_Service_Application.DTOs.Converters;
using System.Text.Json.Serialization;

namespace Order_Service_Application.DTOs
{
    public class GetOrderDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

        [JsonConverter(typeof(DateConverter))]
        public DateTime CreatedAt { get; set; }
        public IEnumerable<GetOrderItemDTO> Items { get; set; }
    }
}
