using MakeMyCapServer.CQS.Interfaces;

namespace MakeMyCapServer.CQS.Command;

public class DeleteSkuCommand : ICommand
{
	public string Sku { get; }

	public DeleteSkuCommand(string sku) => Sku = sku;
}