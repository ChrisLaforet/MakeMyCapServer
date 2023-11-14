namespace MakeMyCapServer.Lookup;

public class SaleProduct
{
    public long ProductId { get; set; }
    public string Title { get; set; }
    public long? InventoryItemId { get; set; }
    public long? LocationId { get; set; }
    public long? VariantId { get; set; }
    public string? Sku { get; set; }
    public int? InventoryLevel { get; set; } 
}