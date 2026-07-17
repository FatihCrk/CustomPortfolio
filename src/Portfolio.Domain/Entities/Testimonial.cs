using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class Testimonial : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string? PhotoUrl { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int Rating { get; set; } = 5; // 1-5
    public bool IsPublished { get; set; } = true;
    public int SortOrder { get; set; } = 0;
}
