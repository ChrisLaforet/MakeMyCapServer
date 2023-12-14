using MakeMyCapServer.Orders;

namespace MakeMyCapServer.Distributors;

public interface IOrderService
{
	bool PlaceOrder(DistributorOrders orders);
}