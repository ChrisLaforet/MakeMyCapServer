using System.Text.Json.Serialization;

namespace MakeMyCapServer.Distributors.SandS.Dtos;

public class PaymentProfile
{
	[JsonPropertyName("email")]
	public string Email { get; set; } = "";
	
	[JsonPropertyName("profileId")]
	public long ProfileId { get; set; }
}