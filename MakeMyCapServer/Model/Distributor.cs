using System;
using System.Collections.Generic;

namespace EFGenerator.Model;

public partial class Distributor
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string CredentialKey { get; set; } = null!;

    public virtual ICollection<SkuDistributor> SkuDistributors { get; set; } = new List<SkuDistributor>();
}
