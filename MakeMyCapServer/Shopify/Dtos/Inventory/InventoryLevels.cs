using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Inventory;

public class InventoryLevels
{
	[JsonPropertyName("inventory_levels")]
	public InventoryLevel[] Items { get; set; }
}