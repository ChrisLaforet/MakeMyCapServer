namespace MakeMyCapServer.Model;

public partial class OrderLineItem
{
    public long LineItemId { get; set; }

    public long FulfillmentOrderId { get; set; }

    public long ShopId { get; set; }

    public int Quantity { get; set; }

    public long InventoryItemId { get; set; }

    public long VariantId { get; set; }

    public virtual FulfillmentOrder FulfillmentOrder { get; set; } = null!;
}
