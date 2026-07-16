using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class ProjectTag : BaseEntity
{
    public int ProjectId { get; set; }
    public string TagName { get; set; } = string.Empty;
    
    // Navigation Properties
    public Project Project { get; set; } = null!;
}
