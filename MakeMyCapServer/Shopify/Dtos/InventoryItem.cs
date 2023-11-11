using System.Text.Json.Serialization;

namespace ShopifyInventoryFulfillment.Shopify.Dtos;

public class InventoryItem
{
	[JsonPropertyName("id")]
	public long Id { get; set; }

	[JsonPropertyName("sku")]
	public string? Sku { get; set; }
	
	[JsonPropertyName("created_at")]
	public DateTime CreatedAt { get; set; }
	
	[JsonPropertyName("updated_at")]
	public DateTime? UpdatedAt { get; set; }

	[JsonPropertyName("requires_shipping")]
	public bool RequiresShipping { get; set; }

	[JsonPropertyName("cost")]
	public string Cost { get; set; }

	[JsonPropertyName("tracked")]	
	public bool Tracked { get; set; }
}