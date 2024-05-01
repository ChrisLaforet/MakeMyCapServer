using System;
using System.Collections.Generic;

namespace MakeMyCapServer.Model;

public partial class PurchaseDistributorOrder
{
    public int Id { get; set; }

    public DateTime CreateDate { get; set; }

    public int DistributorId { get; set; }

    public string Ponumber { get; set; } = null!;
    
    public int? PoNumberSequence { get; set; }

    public long? ShopifyOrderId { get; set; }

    public string Sku { get; set; } = null!;

    public int Quantity { get; set; }

    public string? Style { get; set; }

    public string? Brand { get; set; }
    
    public string? Color { get; set; }

    public string? Size { get; set; }

    public string? Name { get; set; }

    public string? Correlation { get; set; }
    
    public string? ImageOrText { get; set; }
    
    public string? SpecialInstructions { get; set; }
    
    public string? Position { get; set; }
    
    public DateTime SubmittedDateTime { get; set; }

    public DateTime? LastAttemptDateTime { get; set; }

    public DateTime? SuccessDateTime { get; set; }

    public int Attempts { get; set; }

    public int WarningNotificationCount { get; set; }

    public DateTime? FailureNotificationDateTime { get; set; }
    
    public string? ShopifyName { get; set; }
    
    public string? Supplier { get; set; }
    
    public string? SupplierPoNumber { get; set; }

    public string? SupplierPoNumber2 { get; set; }
    
    public string? SupplierPoNumber3 { get; set; }
    
    public virtual Distributor Distributor { get; set; } = null!;
}
