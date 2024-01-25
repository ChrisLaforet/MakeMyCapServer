using System;
using System.Collections.Generic;

namespace MakeMyCapAdmin.Model;

public partial class EmailQueue
{
    public int Id { get; set; }

    public string Sender { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string Body { get; set; } = null!;

    public DateTime PostedDateTime { get; set; }

    public string Recipient { get; set; } = null!;

    public string? Recipient2 { get; set; }

    public string? Recipient3 { get; set; }

    public string? Recipient4 { get; set; }

    public DateTime? LastAttemptDateTime { get; set; }

    public int TotalAttempts { get; set; }

    public DateTime? SentDateTime { get; set; }

    public DateTime? AbandonedDateTime { get; set; }

    public bool BodyIsHtml { get; set; }
}
