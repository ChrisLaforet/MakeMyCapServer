using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class ClientDetails
{
	[JsonPropertyName("accept_language")]
	public object AcceptLanguage { get; set; }

	[JsonPropertyName("browser_height")]
	public object BrowserHeight { get; set; }

	[JsonPropertyName("browser_ip")]
	public string BrowserIp { get; set; }

	[JsonPropertyName("browser_width")]
	public object BrowserWidth { get; set; }

	[JsonPropertyName("session_hash")]
	public object SessionHash { get; set; }

	[JsonPropertyName("user_agent")]
	public object UserAgent { get; set; }
}