using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class Orders
{
	[JsonPropertyName("orders")]
	public Order[] Items { get; set; }
}