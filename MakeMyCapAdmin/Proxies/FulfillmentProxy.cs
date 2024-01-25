using MakeMyCapAdmin.Model;

namespace MakeMyCapAdmin.Proxies;

public class FulfillmentProxy : IFulfillmentProxy
{
	private readonly MakeMyCapServerContext context;
	
	public FulfillmentProxy(MakeMyCapServerContext context)
	{
		this.context = context;
	}
	
	public Order? GetOrderById(long orderId)
	{
		return context.Orders.Find(orderId);
	}

	public bool DoesOrderExist(long orderId)
	{
		return context.Orders.Any(order => order.OrderId == orderId);
	}

	public void SaveOrder(Order order)
	{
		if (!context.Orders.Contains(order))
		{
			context.Orders.Add(order);
		}
		context.SaveChanges();
	}

	public void SaveFullfillmentOrder(FulfillmentOrder fulfillmentOrder)
	{
		if (!context.FulfillmentOrders.Contains(fulfillmentOrder))
		{
			context.FulfillmentOrders.Add(fulfillmentOrder);
		}
		context.SaveChanges();	
	}

	public void SaveOrderLineItem(OrderLineItem orderLineItem)
	{
		if (!context.OrderLineItems.Contains(orderLineItem))
		{
			context.OrderLineItems.Add(orderLineItem);
		}
		context.SaveChanges();
	}
}