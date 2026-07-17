using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? ProfilePhotoUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsSuperAdmin { get; set; } = false;
    public DateTime? LastLoginDate { get; set; }
    public int FailedLoginAttempts { get; set; } = 0;
    public DateTime? LockoutEndDate { get; set; }
    public bool TwoFactorEnabled { get; set; } = false;
    public string? TwoFactorSecret { get; set; }
    
    // Navigation Properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}
