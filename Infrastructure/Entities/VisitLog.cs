using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class VisitLog
{
    public int Id { get; set; }

    public string PageUrl { get; set; } = null!;

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public string? Referrer { get; set; }

    public DateTime VisitDate { get; set; }

    public string VisitorFingerprint { get; set; } = null!;

    public string ClientId { get; set; } = null!;

    public bool IsNewVisitor { get; set; }

    public string SessionId { get; set; } = null!;
}
