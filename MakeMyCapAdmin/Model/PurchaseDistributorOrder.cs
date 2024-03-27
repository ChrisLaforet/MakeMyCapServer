namespace MakeMyCapAdmin.Model;

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

    public string? Color { get; set; }

    public string? Size { get; set; }

    public DateTime SubmittedDateTime { get; set; }

    public DateTime? LastAttemptDateTime { get; set; }

    public DateTime? SuccessDateTime { get; set; }

    public int Attempts { get; set; }

    public int WarningNotificationCount { get; set; }

    public DateTime? FailureNotificationDateTime { get; set; }

    public virtual Distributor Distributor { get; set; } = null!;
}
