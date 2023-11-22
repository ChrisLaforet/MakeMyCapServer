namespace MakeMyCapServer.Model;

public partial class OrderLineItem
{
    public long LineItemId { get; set; }

    public long FulfillmentOrderId { get; set; }

    public int Quantity { get; set; }

    public string Sku { get; set; } = null!;

    public string Name { get; set; } = null!;

    public long ProductId { get; set; }

    public long VariantId { get; set; }

    public string? Ponumber { get; set; }

    public virtual FulfillmentOrder FulfillmentOrder { get; set; } = null!;
}
