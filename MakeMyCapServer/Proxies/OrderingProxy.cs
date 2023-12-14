using MakeMyCapServer.Model;
using Microsoft.EntityFrameworkCore;

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

	public void SavePurchaseOrder(PurchaseDistributorOrder purchaseDistributorOrder)
	{
		if (purchaseDistributorOrder.Id <= 0)
		{
			context.PurchaseOrders.Add(purchaseDistributorOrder);
		}
		context.SaveChanges();
	}

	public List<PurchaseDistributorOrder> GetPurchaseOrders()
	{
		return context.PurchaseOrders
			.Include(po => po.Distributor)
			.ToList();
	}

	public List<PurchaseDistributorOrder> GetPendingPurchaseOrders()
	{
		return context.PurchaseOrders
			.Include(po => po.Distributor)
			.Where(po => po.SuccessDateTime == null && po.FailureNotificationDateTime == null)
			.ToList();
	}

	public int GetMaxPoNumberSequence()
	{
		var max = context.PurchaseOrders.Where(po => po.PoNumberSequence != null)
			.Select(po => po.PoNumberSequence)
			.Max(val => val == null ? 0 : val);
		return max == null ? 0 : (int)max;
	}
}