namespace MakeMyCapServer.Model;

public partial class OrderLineItem
{
    public long LineItemId { get; set; }
    
    public long OrderId { get; set; }

    public int Quantity { get; set; }

    public string Sku { get; set; } = null!;

    public string Name { get; set; } = null!;

    public long ProductId { get; set; }

    public long VariantId { get; set; }

    public string? Ponumber { get; set; }
    
    public string? Correlation { get; set; }
    
    public string? ImageOrText { get; set; }
    
    public string? SpecialInstructions { get; set; }
    
    public string? Position { get; set; }

    public virtual Order Order { get; set; } = null!;
}
