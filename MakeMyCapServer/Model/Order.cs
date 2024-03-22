namespace MakeMyCapServer.Model;

public partial class Order
{
    public long OrderId { get; set; }

    public string OrderNumber { get; set; } = null!;

    public long CheckoutId { get; set; }

    public string? CheckoutToken { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime ProcessStartDateTime { get; set; }
    
    public virtual ICollection<OrderLineItem> OrderLineItems { get; set; } = new List<OrderLineItem>();
}
