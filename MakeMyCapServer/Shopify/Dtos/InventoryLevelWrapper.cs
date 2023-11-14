using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos;

public class InventoryLevelWrapper
{
    [JsonPropertyName("inventory_level")]
    public InventoryLevel Item { get; set; }
}