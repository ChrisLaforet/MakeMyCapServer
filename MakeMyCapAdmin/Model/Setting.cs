using System;
using System.Collections.Generic;

namespace MakeMyCapAdmin.Model;

public partial class Setting
{
    public int Id { get; set; }
    
    public int InventoryCheckHours { get; set; }

    public int FulfillmentCheckHours { get; set; }

    public int? NextPosequence { get; set; }

    public string? StatusEmailRecipient1 { get; set; }

    public string? StatusEmailRecipient2 { get; set; }

    public string? StatusEmailRecipient3 { get; set; }

    public string? CriticalEmailRecipient1 { get; set; }

    public string? CriticalEmailRecipient2 { get; set; }

    public string? CriticalEmailRecipient3 { get; set; }
}
