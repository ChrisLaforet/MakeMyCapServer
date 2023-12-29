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
	
	[JsonPropertyName("closeout")]
	public bool? Closeout { get; set; }
	
	[JsonPropertyName("dropship")]
	public bool? Dropship { get; set; }
	
	[JsonPropertyName("excludeFreeFreight")]
	public bool? ExcludeFreeFreight { get; set; }
	
	[JsonPropertyName("fullCaseOnly")]
	public bool? FullCaseOnly { get; set; }
	
	[JsonPropertyName("returnable")]
	public bool? Returnable { get; set; }
	
	[JsonPropertyName("expectedInventory")]
	public string ExpectedInventory { get; set; }
}