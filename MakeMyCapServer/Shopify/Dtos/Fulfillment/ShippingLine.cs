using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class ShippingLine
{
	[JsonPropertyName("id")]
	public long Id { get; set; }

	[JsonPropertyName("carrier_identifier")]
	public object CarrierIdentifier { get; set; }

	[JsonPropertyName("code")]
	public string Code { get; set; }

	[JsonPropertyName("discounted_price")]
	public string DiscountedPrice { get; set; }

	[JsonPropertyName("discounted_price_set")]
	public AmountSet DiscountedPriceSet { get; set; }

	[JsonPropertyName("phone")]
	public string Phone { get; set; }

	[JsonPropertyName("price")]
	public string Price { get; set; }

	[JsonPropertyName("price_set")]
	public AmountSet PriceSet { get; set; }

	[JsonPropertyName("requested_fulfillment_service_id")]
	public object RequestedFulfillmentServiceId { get; set; }

	[JsonPropertyName("source")]
	public string Source { get; set; }

	[JsonPropertyName("title")]
	public string Title { get; set; }

	[JsonPropertyName("tax_lines")]
	public List<object> TaxLines { get; set; }

	[JsonPropertyName("discount_allocations")]
	public List<object> DiscountAllocations { get; set; }
}