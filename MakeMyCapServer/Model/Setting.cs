using System;
using System.Collections.Generic;

namespace MakeMyCapServer.Model;

public partial class Setting
{
    public int InventoryCheckHours { get; set; }

    public int FulfillmentCheckHours { get; set; }
    
    public int? NextPoSequence { get; set; }
}
