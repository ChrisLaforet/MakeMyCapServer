using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class Money
{
	[JsonPropertyName("amount")]
	public string Amount { get; set; }

	[JsonPropertyName("currency_code")]
	public string CurrencyCode { get; set; }
}