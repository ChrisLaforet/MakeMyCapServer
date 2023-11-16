using MakeMyCapServer.Model;

namespace MakeMyCapServer.Proxies;

public interface IOrderingProxy
{
	Shipping? GetShipping();

	Distributor? GetDistributorByCode(string code);

	void SavePurchaseOrder(PurchaseOrder purchaseOrder);

	List<PurchaseOrder> GetPurchaseOrders();

	List<PurchaseOrder> GetPendingPurchaseOrders();

}