using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class EmailMarketingConsent
{
	[JsonPropertyName("state")]
	public string State { get; set; }

	[JsonPropertyName("opt_in_level")]
	public object OptInLevel { get; set; }

	[JsonPropertyName("consent_updated_at")]
	public DateTime? ConsentUpdatedAt { get; set; }
}