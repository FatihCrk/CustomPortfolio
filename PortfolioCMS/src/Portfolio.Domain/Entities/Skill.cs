using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class Skill : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Level { get; set; } // 0-100
    public int Order { get; set; }
    public string? IconUrl { get; set; }
    public bool IsPublished { get; set; } = true;
}
