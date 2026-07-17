using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class AuditLog : BaseEntity
{
    public Guid? UserId { get; set; }
    public string Action { get; set; } = string.Empty; // Create, Update, Delete, Login, Logout
    public string EntityType { get; set; } = string.Empty;
    public Guid? EntityId { get; set; }
    public string? OldValues { get; set; } // JSON
    public string? NewValues { get; set; } // JSON
    public string IpAddress { get; set; } = string.Empty;
    public string? UserAgent { get; set; }
    public string? BrowserInfo { get; set; }
    public string? DeviceInfo { get; set; }
    
    // Navigation Properties
    public User? User { get; set; }
}
