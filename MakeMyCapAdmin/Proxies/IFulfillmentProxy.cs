using MakeMyCapAdmin.Model;

namespace MakeMyCapAdmin.Proxies;

public interface IFulfillmentProxy
{
	Order? GetOrderById(long orderId);
	bool DoesOrderExist(long orderId);
	void SaveOrder(Order order);

	void SaveFullfillmentOrder(FulfillmentOrder fulfillmentOrder);

	void SaveOrderLineItem(OrderLineItem orderLineItem);
}