using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class SmsMarketingConsent
{
	[JsonPropertyName("state")]
	public string State { get; set; }

	[JsonPropertyName("opt_in_level")]
	public string OptInLevel { get; set; }

	[JsonPropertyName("consent_updated_at")]
	public DateTime? ConsentUpdatedAt { get; set; }

	[JsonPropertyName("consent_collected_from")]
	public string ConsentCollectedFrom { get; set; }
}