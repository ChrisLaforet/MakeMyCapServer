using System.Text.Json.Serialization;

namespace ShopifyInventoryFulfillment.Shopify.Dtos;

public class InventoryItemWrapper
{
	[JsonPropertyName("inventory_item")]
	public InventoryItem InventoryItem { get; set; }
}