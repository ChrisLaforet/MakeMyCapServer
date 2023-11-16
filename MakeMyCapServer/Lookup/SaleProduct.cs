using MakeMyCapServer.Model;

namespace MakeMyCapServer.Lookup;

public class SaleProduct
{
    public Product Product { get; private set; }

    public SaleProduct(Product product)
    {
        this.Product = product;
    }

    public long ProductId => Product.ProductId;
    public string Title => Product.Title;
    public long? InventoryItemId => Product.InventoryItemId;
    public long? LocationId { get; set; }
    public long? VariantId => Product.VariantId;
    public string? Sku => Product.Sku;
    public int? InventoryLevel { get; set; } 
}