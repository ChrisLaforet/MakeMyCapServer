namespace MakeMyCapServer.Orders;

public interface IDistributorOrder
{
	int Id { get; }

	DateTime OrderDate { get; }

	string PoNumber { get; }

	long? ShopifyOrderId { get; }
	
	string Sku { get; }

	int Quantity { get; }

	string? Style { get; }

	string? Color { get; }

	string? Size { get; }
	
	string DistributorName { get; }

	DateTime CreateDate { get; }
	
	DateTime? SuccessDateTime { get; set; }
	
	DateTime? LastAttemptDateTime { get; set; }

	int Attempts { get; set; }

	int WarningNotificationCount { get; set; }

	DateTime? FailureNotificationDateTime { get; set; }
}