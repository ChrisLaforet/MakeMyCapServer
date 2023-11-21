using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class Receipt
{
	[JsonPropertyName("testcase")]
	public bool Testcase { get; set; }

	[JsonPropertyName("authorization")]
	public string Authorization { get; set; }
}