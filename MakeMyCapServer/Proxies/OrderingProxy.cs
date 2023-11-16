using MakeMyCapServer.Model;

namespace MakeMyCapServer.Proxies;

public class OrderingProxy : IOrderingProxy
{
	private readonly MakeMyCapServerContext context;
	
	public OrderingProxy(MakeMyCapServerContext context)
	{
		this.context = context;
	}
	
	public Shipping? GetShipping()
	{
		return context.Shippings.FirstOrDefault();
	}

	public Distributor? GetDistributorByCode(string code)
	{
		return context.Distributors.FirstOrDefault(distributor => string.Compare(distributor.LookupCode, code, true) == 0);
	}

	public void SavePurchaseOrder(PurchaseOrder purchaseOrder)
	{
		if (purchaseOrder.Id <= 0)
		{
			context.PurchaseOrders.Add(purchaseOrder);
		}
		context.SaveChanges();
	}

	public List<PurchaseOrder> GetPurchaseOrders()
	{
		return context.PurchaseOrders.ToList();
	}

	public List<PurchaseOrder> GetIncompletePurchaseOrders()
	{
		return context.PurchaseOrders.Where(po => po.SuccessDateTime == null).ToList();
	}
}