using System.Text.Json.Serialization;

namespace ShopifyInventoryFulfillment.Shopify.Dtos;

public class InventoryLevelWrapper
{
    [JsonPropertyName("inventory_level")]
    public InventoryLevel Item { get; set; }
}