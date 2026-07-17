using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class MenuItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Url { get; set; }
    public int SortOrder { get; set; } = 0;
    public Guid? ParentId { get; set; }
    public bool IsPublished { get; set; } = true;
    public string? IconUrl { get; set; }
    public bool OpenInNewTab { get; set; } = false;
    
    // Navigation Properties
    public MenuItem? Parent { get; set; }
    public ICollection<MenuItem> Children { get; set; } = new List<MenuItem>();
}
