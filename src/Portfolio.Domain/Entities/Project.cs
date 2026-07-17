using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class Project : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? FullDescription { get; set; }
    public string? Category { get; set; }
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public bool IsFeatured { get; set; } = false;
    public int SortOrder { get; set; } = 0;
    public bool IsPublished { get; set; } = true;
    
    // SEO
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    
    // Navigation Properties
    public ICollection<ProjectImage> Images { get; set; } = new List<ProjectImage>();
    public ICollection<ProjectTechnology> Technologies { get; set; } = new List<ProjectTechnology>();
    public ICollection<ProjectTag> Tags { get; set; } = new List<ProjectTag>();
}
