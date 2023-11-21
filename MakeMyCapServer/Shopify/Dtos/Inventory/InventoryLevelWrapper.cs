using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Inventory;

public class InventoryLevelWrapper
{
    [JsonPropertyName("inventory_level")]
    public InventoryLevel Item { get; set; }
}