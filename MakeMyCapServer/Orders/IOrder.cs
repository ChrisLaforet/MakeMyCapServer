﻿namespace MakeMyCapServer.Orders;

public interface IOrder
{
	DateTime OrderDate { get; }
	string PoNumber { get; }
	List<IOrderItem> LineItems { get; }
	long? ShopifyOrderId { get; }
	
	string DistributorName { get; }
	DateTime? SuccessDateTime { get; set; }
}