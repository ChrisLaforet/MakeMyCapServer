using System.Text.Json.Serialization;

namespace MakeMyCapServer.Distributors.SandS.Dtos;

public class Warehouse
{
	[JsonPropertyName("warehouseAbbr")]
	public string WarehouseAbbreviation { get; set; }
	
	[JsonPropertyName("skuID")]
	public long SkuId { get; set; }
		
	[JsonPropertyName("qty")]
	public long Quantity { get; set; }
}