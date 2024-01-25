using System;
using System.Collections.Generic;

namespace MakeMyCapAdmin.Model;

public partial class Distributor
{
    public int Id { get; set; }

    public string LookupCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? AccountNumber { get; set; }

    public virtual ICollection<PurchaseDistributorOrder> PurchaseOrders { get; set; } = new List<PurchaseDistributorOrder>();
}
