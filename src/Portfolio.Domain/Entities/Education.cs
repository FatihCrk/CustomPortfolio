using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class Education : BaseEntity
{
    public string SchoolName { get; set; } = string.Empty;
    public string? SchoolLogoUrl { get; set; }
    public string Degree { get; set; } = string.Empty;
    public string? FieldOfStudy { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; } = 0;
}
