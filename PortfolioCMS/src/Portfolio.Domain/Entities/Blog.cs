using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class Blog : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Category { get; set; }
    public string? CoverImageUrl { get; set; }
    public int AuthorId { get; set; }
    public DateTime PublishedDate { get; set; }
    public bool IsDraft { get; set; } = true;
    public bool IsPublished { get; set; }
    public int ViewCount { get; set; }
    
    // SEO
    public string? Slug { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    
    // Navigation Properties
    public User? Author { get; set; }
    public ICollection<BlogTag> Tags { get; set; } = new List<BlogTag>();
}
