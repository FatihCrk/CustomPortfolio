using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class FeatureFlag : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public DateTime? EnabledUntil { get; set; }
}
