using System;
using System.Collections.Generic;

namespace MakeMyCapServer.Model;

public partial class Distributor
{
    public int Id { get; set; }

    public string LookupCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? AccountNumber { get; set; }

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    public virtual ICollection<SkuDistributor> SkuDistributors { get; set; } = new List<SkuDistributor>();
}
