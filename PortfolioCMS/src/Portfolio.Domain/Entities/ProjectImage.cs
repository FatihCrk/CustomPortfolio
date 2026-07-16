using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class ProjectImage : BaseEntity
{
    public int ProjectId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int Order { get; set; }
    public bool IsCover { get; set; }
    
    // Navigation Properties
    public Project Project { get; set; } = null!;
}
