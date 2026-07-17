using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class SocialMedia : BaseEntity
{
    public string Platform { get; set; } = string.Empty; // GitHub, LinkedIn, etc.
    public string Url { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public int SortOrder { get; set; } = 0;
    public bool IsPublished { get; set; } = true;
}
