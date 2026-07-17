using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class Session : BaseEntity
{
    public Guid UserId { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public string? DeviceInfo { get; set; }
    public string? BrowserInfo { get; set; }
    public string? IpAddress { get; set; }
    public DateTime LastActivityDate { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation Properties
    public User User { get; set; } = null!;
}
