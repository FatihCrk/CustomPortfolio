using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class BlogTag : BaseEntity
{
    public Guid BlogPostId { get; set; }
    public string TagName { get; set; } = string.Empty;
    
    // Navigation Properties
    public BlogPost BlogPost { get; set; } = null!;
}
