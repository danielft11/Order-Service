using Order_Service_Application.DTOs.Converters;
using System.Text.Json.Serialization;

namespace Order_Service_Application.DTOs
{
    public class GetOrderItemDTO
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        [JsonConverter(typeof(CurrencyConverter))]
        public decimal TotalValue { get; set; }
    }
}
