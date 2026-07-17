using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class EmailTemplate : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty; // HTML
    public string? PlainTextBody { get; set; }
    public bool IsActive { get; set; } = true;
    public string Category { get; set; } = "General";
}
