using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class Property
{
	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("value")]
	public string Value { get; set; }
}