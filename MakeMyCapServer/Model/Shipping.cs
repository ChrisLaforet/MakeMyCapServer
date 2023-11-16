using System;
using System.Collections.Generic;

namespace MakeMyCapServer.Model;

public partial class Shipping
{
    public int Id { get; set; }

    public string? ShipTo { get; set; }

    public string ShipAddress1 { get; set; } = null!;

    public string? ShipAddress2 { get; set; }

    public string ShipCity { get; set; } = null!;

    public string ShipState { get; set; } = null!;

    public string ShipZip { get; set; } = null!;

    public string ShipEmail { get; set; } = null!;

    public string ShipMethod { get; set; } = null!;

    public string? Attention { get; set; }
}
