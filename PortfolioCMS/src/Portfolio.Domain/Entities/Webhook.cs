using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class Webhook : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string? Secret { get; set; }
    public bool IsActive { get; set; } = true;
    public int FailureCount { get; set; }
    public DateTime? LastTriggeredAt { get; set; }
}
