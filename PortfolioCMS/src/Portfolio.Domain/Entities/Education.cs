using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class Education : BaseEntity
{
    public string SchoolName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Degree { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; }
    public bool IsPublished { get; set; } = true;
}
