using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class Service : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public int Order { get; set; }
    public bool IsPublished { get; set; } = true;
}
