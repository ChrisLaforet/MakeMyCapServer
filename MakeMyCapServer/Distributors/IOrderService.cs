using MakeMyCapServer.Orders;

namespace MakeMyCapServer.Distributors;

public interface IOrderService
{
	bool PlaceOrder(IOrder order);
}