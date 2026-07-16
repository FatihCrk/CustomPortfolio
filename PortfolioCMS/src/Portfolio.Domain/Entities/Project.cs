using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class Project : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Category { get; set; }
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public int Order { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; } = true;
    
    // SEO
    public string? Slug { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    
    // Navigation Properties
    public ICollection<ProjectImage> Images { get; set; } = new List<ProjectImage>();
    public ICollection<ProjectTechnology> Technologies { get; set; } = new List<ProjectTechnology>();
    public ICollection<ProjectTag> Tags { get; set; } = new List<ProjectTag>();
}
