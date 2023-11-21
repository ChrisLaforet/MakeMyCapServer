using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class DiscountCode
{
	[JsonPropertyName("code")]
	public string Code { get; set; }

	[JsonPropertyName("amount")]
	public string Amount { get; set; }

	[JsonPropertyName("type")]
	public string Type { get; set; }
}