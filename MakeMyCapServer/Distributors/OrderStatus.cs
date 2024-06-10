using MakeMyCapServer.Services.OrderPlacement;

namespace MakeMyCapServer.Distributors;

public class OrderStatus
{
	public bool Successful { get; private set; }
	public List<IOutOfStockItem> OutOfStockItems = new List<IOutOfStockItem>();

	public OrderStatus(bool iSuccessful, List<IOutOfStockItem>? outOfStockItems = null)
	{
		Successful = iSuccessful;
		if (outOfStockItems != null)
		{
			OutOfStockItems = outOfStockItems;
		}
	}
}