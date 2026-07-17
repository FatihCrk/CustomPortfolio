using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class FeatureFlag : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsEnabled { get; set; } = false;
    public DateTime? EnabledFrom { get; set; }
    public DateTime? EnabledUntil { get; set; }
}
