using MakeMyCapServer.Controllers.Model;
using MakeMyCapServer.CQS.Interfaces;


namespace MakeMyCapServer.CQS.Command;

public class UpdateSkuCommand : ICommand
{
	public string OriginalSku { get; }

	public string NewSku { get; }

	public string? DistributorSku { get; }

	public string? Brand { get; }

	public string StyleCode { get; }

	public string? PartId { get; }

	public string? Color { get; }

	public string? ColorCode { get; }

	public string? SizeCode { get; }

	public UpdateSkuCommand(UpdateSku request)
	{
		OriginalSku = request.OriginalSku;
		NewSku = request.NewSku;
		DistributorSku = request.DistributorSku;
		Brand = request.Brand;
		StyleCode = request.StyleCode;
		PartId = request.PartId;
		Color = request.Color;
		ColorCode = request.ColorCode;
		SizeCode = request.SizeCode;
	}
}