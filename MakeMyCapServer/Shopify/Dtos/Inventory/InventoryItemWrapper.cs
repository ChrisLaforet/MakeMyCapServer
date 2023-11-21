using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Inventory;

public class InventoryItemWrapper
{
	[JsonPropertyName("inventory_item")]
	public InventoryItem InventoryItem { get; set; }
}