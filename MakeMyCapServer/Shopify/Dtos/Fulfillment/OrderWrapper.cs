using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class OrderWrapper
{
	[JsonPropertyName("order")]
	public Order Order { get; set; }
}