using System;
using System.Collections.Generic;

namespace MakeMyCapServer.Model;

public partial class DistributorSkuMap
{
    public string Sku { get; set; } = null!;

    public string DistributorCode { get; set; } = null!;

    public string? DistributorSku { get; set; }

    public string? Brand { get; set; }

    public string StyleCode { get; set; }

    public string? PartId { get; set; }

    public string? Color { get; set; }

    public string? ColorCode { get; set; }

    public string? SizeCode { get; set; }
}
