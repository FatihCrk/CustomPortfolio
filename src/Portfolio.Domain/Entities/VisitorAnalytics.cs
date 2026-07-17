using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class VisitorAnalytics : BaseEntity
{
    public string IpAddress { get; set; } = string.Empty;
    public string? UserAgent { get; set; }
    public string? BrowserInfo { get; set; }
    public string? DeviceInfo { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Referrer { get; set; }
    public string? PageUrl { get; set; }
    public DateTime VisitDate { get; set; } = DateTime.UtcNow;
    public int SessionDuration { get; set; } // seconds
}
