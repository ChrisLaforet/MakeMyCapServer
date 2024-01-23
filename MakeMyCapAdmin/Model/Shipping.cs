using System;
using System.Collections.Generic;

namespace MakeMyCapServer.Model;

public partial class Shipping
{
    public int Id { get; set; }

    public string? ShipTo { get; set; }

    public string Name { get; set; } = null!;

    public string ShipAddress { get; set; } = null!;

    public string ShipCity { get; set; } = null!;

    public string ShipState { get; set; } = null!;

    public string ShipZip { get; set; } = null!;

    public string? ShipEmail { get; set; }

    public string ShipMethod { get; set; } = null!;

    public string? Attention { get; set; }
}
