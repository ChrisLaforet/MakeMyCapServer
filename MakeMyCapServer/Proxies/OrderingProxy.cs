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
		return context.Distributors.FirstOrDefault(distributor => distributor.LookupCode.ToUpper() == code.ToUpper());
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

	public List<PurchaseOrder> GetPendingPurchaseOrders()
	{
		return context.PurchaseOrders.Where(po => po.SuccessDateTime == null && po.FailureNotificationDateTime == null).ToList();
	}

	public int GetMaxPoNumberSequence()
	{
		var max = context.PurchaseOrders.Where(po => po.PoNumberSequence != null)
			.Select(po => po.PoNumberSequence)
			.Max(val => val == null ? 0 : val);
		return max == null ? 0 : (int)max;
	}
}