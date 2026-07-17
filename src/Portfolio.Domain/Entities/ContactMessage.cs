using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class ContactMessage : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string? BrowserInfo { get; set; }
    public string? DeviceInfo { get; set; }
    public bool IsRead { get; set; } = false;
    public bool IsArchived { get; set; } = false;
    public DateTime? RespondedDate { get; set; }
    public string? AdminResponse { get; set; }
}
