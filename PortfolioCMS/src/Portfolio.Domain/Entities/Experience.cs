using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class Experience : BaseEntity
{
    public string CompanyName { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string Position { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrentPosition { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsPublished { get; set; } = true;
}
