using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class ProjectImage : BaseEntity
{
    public Guid ProjectId { get; set; }
    public Guid MediaFileId { get; set; }
    public int SortOrder { get; set; } = 0;
    public bool IsCover { get; set; } = false;
    
    // Navigation Properties
    public Project Project { get; set; } = null!;
    public MediaFile MediaFile { get; set; } = null!;
}
