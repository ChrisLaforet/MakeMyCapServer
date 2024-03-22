using MakeMyCapServer.Model;

namespace MakeMyCapServer.Proxies;

public interface IOrderingProxy
{
	Shipping? GetShipping();

	Distributor? GetDistributorByCode(string code);

	List<Distributor> GetDistributors();

	void SavePurchaseOrder(PurchaseDistributorOrder purchaseDistributorOrder);

	List<PurchaseDistributorOrder> GetPurchaseOrders();

	List<PurchaseDistributorOrder> GetPendingPurchaseOrders();

	int GetMaxPoNumberSequence();
	
	Order? GetOrderById(long orderId);
	bool DoesOrderExist(long orderId);
	void SaveOrder(Order order);

	void SaveOrderLineItem(OrderLineItem orderLineItem);
}