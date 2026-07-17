using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class Experience : BaseEntity
{
    public string CompanyName { get; set; } = string.Empty;
    public string? CompanyLogoUrl { get; set; }
    public string Position { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrentPosition { get; set; } = false;
    public string? Description { get; set; }
    public int SortOrder { get; set; } = 0;
}
