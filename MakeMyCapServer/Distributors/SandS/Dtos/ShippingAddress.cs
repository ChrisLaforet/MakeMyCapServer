using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace MakeMyCapServer.Distributors.SandS.Dtos;

public class ShippingAddress
{
	[JsonPropertyName("customer")]
	public string Customer { get; set; } = "";
	
	[JsonPropertyName("attn")]
	public string Attn { get; set; } = "";
	
	[JsonPropertyName("address")]
	public string Address { get; set; }
		
	[JsonPropertyName("city")]
	public string City { get; set; }
		
	[JsonPropertyName("state")]
	public string State { get; set; }
		
	[JsonPropertyName("zip")]
	public string Zip { get; set; }

	[JsonPropertyName("residential")] 
	public bool Residential { get; set; } = false;
}