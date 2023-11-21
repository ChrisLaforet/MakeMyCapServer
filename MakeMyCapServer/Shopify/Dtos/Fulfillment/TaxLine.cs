using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class TaxLine
{
	[JsonPropertyName("price")]
	public string Price { get; set; }

	[JsonPropertyName("rate")]
	public double Rate { get; set; }

	[JsonPropertyName("title")]
	public string Title { get; set; }

	[JsonPropertyName("price_set")]
	public AmountSet PriceSet { get; set; }

	[JsonPropertyName("channel_liable")]
	public object ChannelLiable { get; set; }
}