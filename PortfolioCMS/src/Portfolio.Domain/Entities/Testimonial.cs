using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class Testimonial : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsPublished { get; set; } = true;
}
