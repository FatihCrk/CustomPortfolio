using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class HeroSection : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string SubTitle { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? BackgroundImageUrl { get; set; }
    public string? ButtonText { get; set; }
    public string? ButtonUrl { get; set; }
    public bool ShowButton { get; set; } = true;
    public int Order { get; set; }
    public bool IsPublished { get; set; } = true;
}
