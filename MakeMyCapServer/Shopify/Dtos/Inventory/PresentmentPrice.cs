using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Inventory;

public class PresentmentPrice
{
    [JsonPropertyName("price")]
    public Price Price { get; set; }

    [JsonPropertyName("compare_at_price")]
    public object CompareAtPrice { get; set; }
}