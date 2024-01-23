namespace MakeMyCapServer.Model;

public partial class FulfillmentOrder
{
    public long FulfillmentOrderId { get; set; }

    public long OrderId { get; set; }

    public string Status { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual ICollection<OrderLineItem> OrderLineItems { get; set; } = new List<OrderLineItem>();
}
