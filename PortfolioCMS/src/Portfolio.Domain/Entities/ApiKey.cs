using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class ApiKey : BaseEntity
{
    public int UserId { get; set; }
    public string KeyName { get; set; } = string.Empty;
    public string KeyHash { get; set; } = string.Empty;
    public string KeyPrefix { get; set; } = string.Empty; // First 8 chars for identification
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? LastUsedAt { get; set; }
    public string? Permissions { get; set; } // JSON serialized permissions
    
    // Navigation Properties
    public User User { get; set; } = null!;
}
