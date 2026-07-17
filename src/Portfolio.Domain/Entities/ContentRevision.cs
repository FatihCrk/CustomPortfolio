using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class ContentRevision : BaseEntity
{
    public Guid EntityId { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public string ContentSnapshot { get; set; } = string.Empty; // JSON
    public Guid RevisedBy { get; set; }
    public string? ChangeDescription { get; set; }
    public int VersionNumber { get; set; }
    
    // Navigation Properties
    public User RevisedByUser { get; set; } = null!;
}
