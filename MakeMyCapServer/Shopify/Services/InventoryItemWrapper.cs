using System.Text.Json.Serialization;
using ShopifyInventoryFulfillment.Shopify.Dtos;

namespace ShopifyInventoryFulfillment.Shopify;

public class InventoryItemWrapper
{
	[JsonPropertyName("inventory_item")]
	public InventoryItem Item;
}