using System;
using System.Collections.Generic;

namespace MakeMyCap.Model;

public partial class ServiceLog
{
    public int Id { get; set; }

    public string ServiceName { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }
    
    public bool? Failed { get; set; }
}
