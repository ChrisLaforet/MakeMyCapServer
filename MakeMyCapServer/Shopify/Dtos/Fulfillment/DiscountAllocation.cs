using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class DiscountAllocation
{
	[JsonPropertyName("amount")]
	public string Amount { get; set; }

	[JsonPropertyName("amount_set")]
	public AmountSet AmountSet { get; set; }

	[JsonPropertyName("discount_application_index")]
	public int DiscountApplicationIndex { get; set; }
}