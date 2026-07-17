using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class ProjectTechnology : BaseEntity
{
    public Guid ProjectId { get; set; }
    public string TechnologyName { get; set; } = string.Empty;
    
    // Navigation Properties
    public Project Project { get; set; } = null!;
}
