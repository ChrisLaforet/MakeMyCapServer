using System.Text.Json.Serialization;

namespace MakeMyCapServer.Distributors.SandS.Dtos;

public class Line
{
	[JsonPropertyName("identifier")] 
	public String Identifier { get; set; }
	
	[JsonPropertyName("qty")] 
	public int Qty { get; set; }
}