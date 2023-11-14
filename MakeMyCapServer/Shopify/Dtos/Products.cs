using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos;

public class Products
{
    [JsonPropertyName("products")]
    public List<Product> Items { get; set; }
}