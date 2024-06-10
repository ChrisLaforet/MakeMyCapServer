namespace MakeMyCapServer.Orders;

public interface IDistributorOrder
{
	int Id { get; }

	DateTime OrderDate { get; }

	string PoNumber { get; }
	
	int? PoNumberSequence { get; }

	long? ShopifyOrderId { get; }
	
	string Sku { get; }

	int Quantity { get; set; }

	string? Style { get; }
	
	string? Brand { get; }

	string? Color { get; }

	string? Size { get; }
	
	string DistributorName { get; }
	
	string Name { get; }
	
	string Correlation { get; }
	
	string ImageOrText { get; }
	
	string Position { get; }
	
	string SpecialInstructions { get; }

	DateTime CreateDate { get; }
	
	DateTime? SuccessDateTime { get; set; }
	
	DateTime? LastAttemptDateTime { get; set; }

	int Attempts { get; set; }

	int WarningNotificationCount { get; set; }

	DateTime? FailureNotificationDateTime { get; set; }
	
	string? ShopifyName { get; set; }
	
	string? Supplier { get; set; }

	string? SupplierPoNumber { get; set; }

	string? SupplierPoNumber2 { get; set; }

	string? SupplierPoNumber3 { get; set; }
}