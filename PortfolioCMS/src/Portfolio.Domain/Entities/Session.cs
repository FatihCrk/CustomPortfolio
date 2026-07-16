using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class Session : BaseEntity
{
    public int UserId { get; set; }
    public string SessionToken { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string Browser { get; set; } = string.Empty;
    public string Device { get; set; } = string.Empty;
    public DateTime LastActivity { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation Properties
    public User User { get; set; } = null!;
}
