using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class BlogTag : BaseEntity
{
    public int BlogId { get; set; }
    public string TagName { get; set; } = string.Empty;
    
    // Navigation Properties
    public Blog Blog { get; set; } = null!;
}
