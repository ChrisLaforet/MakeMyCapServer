using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Inventory;

public class Products
{
    [JsonPropertyName("products")]
    public List<Product> Items { get; set; }
}