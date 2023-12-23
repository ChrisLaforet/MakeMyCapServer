using MakeMyCapServer.Controllers.Model;

namespace MakeMyCapServer.CQS.Command;

public class CreateSkuCommand : ICommand
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

	public CreateSkuCommand(CreateSku request)
	{
		Sku = request.Sku;
		DistributorCode = request.DistributorCode;
		DistributorSku = request.DistributorSku;
		Brand = request.Brand;
		StyleCode = request.StyleCode;
		PartId = request.PartId;
		Color = request.Color;
		ColorCode = request.ColorCode;
		SizeCode = request.SizeCode;
	}
}