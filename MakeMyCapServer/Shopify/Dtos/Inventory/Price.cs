using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Inventory;

public class Price
{
    [JsonPropertyName("amount")]
    public string Amount { get; set; }

    [JsonPropertyName("currency_code")]
    public string CurrencyCode { get; set; }
}