using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Order_Service_Application.DTOs.Converters
{
    public class CurrencyConverter : JsonConverter<decimal>
    {
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => decimal.Parse(reader.GetString(), new CultureInfo("pt-BR"));

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString("C", new CultureInfo("pt-BR")));
    }
}
