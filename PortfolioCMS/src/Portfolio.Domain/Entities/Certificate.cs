using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class Certificate : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? FileUrl { get; set; }
    public DateTime? IssueDate { get; set; }
    public string? IssuingOrganization { get; set; }
    public int Order { get; set; }
    public bool IsPublished { get; set; } = true;
}
