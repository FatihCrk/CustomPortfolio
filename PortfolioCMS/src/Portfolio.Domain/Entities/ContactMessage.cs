using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class ContactMessage : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string Browser { get; set; } = string.Empty;
    public string Device { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public bool IsArchived { get; set; }
    public string? Response { get; set; }
    public DateTime? RespondedAt { get; set; }
}
