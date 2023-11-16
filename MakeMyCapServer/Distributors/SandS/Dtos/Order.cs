using System.Text.Json.Serialization;

namespace MakeMyCapServer.Distributors.SandS.Dtos;

public class Order
{
	[JsonPropertyName("shippingAddress")]
	public ShippingAddress ShippingAddress { get; set; }
	
	[JsonPropertyName("shippingMethod")] 
	public string ShippingMethod { get; set; }

	[JsonPropertyName("poNumber")] 
	public string PoNumber { get; set; }
	
	[JsonPropertyName("emailConfirmation")] 
	public string EmailConfirmation { get; set; } = "";

	[JsonPropertyName("testOrder")] 
	public bool? TestOrder { get; set; } = false;

	[JsonPropertyName("autoselectWarehouse")]
	public bool AutoselectWarehouse { get; set; } = true;

	[JsonPropertyName("lines")] 
	public List<Line> Lines { get; set; } = new List<Line>();
}