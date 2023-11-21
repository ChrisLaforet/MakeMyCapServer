using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class DiscountApplication
{
	[JsonPropertyName("target_type")]
	public string TargetType { get; set; }

	[JsonPropertyName("type")]
	public string Type { get; set; }

	[JsonPropertyName("value")]
	public string Value { get; set; }

	[JsonPropertyName("value_type")]
	public string ValueType { get; set; }

	[JsonPropertyName("allocation_method")]
	public string AllocationMethod { get; set; }

	[JsonPropertyName("target_selection")]
	public string TargetSelection { get; set; }

	[JsonPropertyName("code")]
	public string Code { get; set; }
}