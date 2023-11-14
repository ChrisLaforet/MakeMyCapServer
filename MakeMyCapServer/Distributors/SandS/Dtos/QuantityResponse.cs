using System.Text.Json.Serialization;

namespace MakeMyCapServer.Distributors.SandS.Dtos;

public class QuantityResponse
{
	[JsonPropertyName("sku")]
	public string Sku { get; set; }
	
	[JsonPropertyName("gtin")]
	public string Gtin { get; set; }
	
	[JsonPropertyName("skuID_Master")]
	public long SkuIdMaster { get; set; }
	
	[JsonPropertyName("yourSku")]
	public string YourSku { get; set; }
	
	[JsonPropertyName("styleId")]
	public long StyleId { get; set; }
	
	[JsonPropertyName("warehouses")]
	public Warehouse[] Warehouses { get; set; }
}