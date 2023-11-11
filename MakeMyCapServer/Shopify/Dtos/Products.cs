using System.Text.Json.Serialization;

namespace ShopifyInventoryFulfillment.Shopify.Dtos;

public class Products
{
    [JsonPropertyName("products")]
    public List<Product> Items { get; set; }
}