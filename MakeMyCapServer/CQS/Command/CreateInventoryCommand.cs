using MakeMyCapServer.Controllers.Model;
using MakeMyCapServer.CQS.Interfaces;

namespace MakeMyCapServer.CQS.Command;

public class CreateInventoryCommand : ICommand
{
	public string Sku { get; }
	public int OnHand { get; }

	public CreateInventoryCommand(CreateInventory request)
	{
		this.Sku = request.Sku;
		this.OnHand = request.OnHand;
	}
}