using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos;

public class InventoryItemWrapper
{
	[JsonPropertyName("inventory_item")]
	public InventoryItem InventoryItem { get; set; }
}