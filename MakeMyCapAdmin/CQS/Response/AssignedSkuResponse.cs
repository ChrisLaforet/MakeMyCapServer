using MakeMyCapServer.Model;

namespace MakeMyCapServer.CQS.Response;

public class AssignedSkuResponse
{
	public string Sku { get; }
	public string DistributorCode { get; }
	public string? DistributorSku { get; }
	public string? Brand { get; }
	public string StyleCode { get; }
	public string? PartId { get; }
	public string? Color { get; }
	public string? ColorCode { get; }
	public string? SizeCode { get; }

	public AssignedSkuResponse(DistributorSkuMap map)
	{
		Sku = map.Sku;
		DistributorCode = map.DistributorCode;
		DistributorSku = map.DistributorSku;
		Brand = map.Brand;
		StyleCode = map.StyleCode;
		PartId = map.PartId;
		Color = map.Color;
		ColorCode = map.ColorCode;
		SizeCode = map.SizeCode;
	}
}