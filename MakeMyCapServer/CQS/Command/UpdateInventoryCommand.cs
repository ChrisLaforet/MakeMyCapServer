using MakeMyCapServer.Controllers.Model;
using MakeMyCapServer.CQS.Interfaces;

namespace MakeMyCapServer.CQS.Command;

public class UpdateInventoryCommand : ICommand
{
	public string Sku { get; }
	public int OnHand { get; }

	public UpdateInventoryCommand(UpdateInventory request)
	{
		this.Sku = request.Sku;
		this.OnHand = request.OnHand;
	}
}