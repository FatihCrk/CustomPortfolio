using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class SocialMedia : BaseEntity
{
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public int Order { get; set; }
    public bool IsPublished { get; set; } = true;
}
