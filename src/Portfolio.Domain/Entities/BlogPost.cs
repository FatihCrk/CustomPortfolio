using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class BlogPost : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? Summary { get; set; }
    public Guid AuthorId { get; set; }
    public string? Category { get; set; }
    public Guid? CoverImageId { get; set; }
    public bool IsPublished { get; set; } = false;
    public bool IsDraft { get; set; } = true;
    public DateTime? PublishedDate { get; set; }
    public int ViewCount { get; set; } = 0;
    
    // SEO
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    
    // Navigation Properties
    public User Author { get; set; } = null!;
    public MediaFile? CoverImage { get; set; }
    public ICollection<BlogTag> Tags { get; set; } = new List<BlogTag>();
}
