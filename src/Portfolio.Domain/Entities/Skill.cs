using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class Skill : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public int Percentage { get; set; } // 0-100
    public int SortOrder { get; set; } = 0;
    public string? IconUrl { get; set; }
    public bool IsPublished { get; set; } = true;
}
