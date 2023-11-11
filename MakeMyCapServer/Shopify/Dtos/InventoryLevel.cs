using System.Text.Json.Serialization;

namespace ShopifyInventoryFulfillment.Shopify.Dtos;

public class InventoryLevel
{
	[JsonPropertyName("inventory_item_id")]
	public long InventoryItemId { get; set; }
	
	[JsonPropertyName("location_id")]
	public long LocationId { get; set; }
	
	[JsonPropertyName("available")]
	public int? Available { get; set; }
	
	[JsonPropertyName("updated_at")]
	public DateTime UpdatedAt { get; set; }
}