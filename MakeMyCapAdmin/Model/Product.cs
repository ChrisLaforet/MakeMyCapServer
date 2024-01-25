using System;
using System.Collections.Generic;

namespace MakeMyCapAdmin.Model;

public partial class Product
{
    public int Id { get; set; }

    public long VariantId { get; set; }

    public long ProductId { get; set; }

    public string Sku { get; set; } = null!;

    public string? Title { get; set; }

    public string? Vendor { get; set; }
    
    public long InventoryItemId { get; set; }
}
