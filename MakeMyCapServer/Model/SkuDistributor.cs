using System;
using System.Collections.Generic;

namespace MakeMyCapServer.Model;

public partial class SkuDistributor
{
    public int Id { get; set; }

    public string Sku { get; set; } = null!;

    public int DistributorId { get; set; }

    public virtual Distributor Distributor { get; set; } = null!;
}
