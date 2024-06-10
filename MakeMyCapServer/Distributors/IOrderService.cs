using MakeMyCapServer.Orders;

namespace MakeMyCapServer.Distributors;

public interface IOrderService
{
	OrderStatus PlaceOrder(DistributorOrders orders);
}