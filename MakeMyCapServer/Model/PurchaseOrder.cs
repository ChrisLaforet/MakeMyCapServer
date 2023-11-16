using System;
using System.Collections.Generic;

namespace MakeMyCapServer.Model;

public partial class PurchaseOrder
{
    public int Id { get; set; }

    public DateTime CreateDate { get; set; }

    public int DistributorId { get; set; }

    public string Ponumber { get; set; } = null!;

    public long? ShopifyOrderId { get; set; }

    public string Sku { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual Distributor Distributor { get; set; } = null!;
}
