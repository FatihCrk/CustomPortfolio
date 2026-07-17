using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? RevokedDate { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? ReasonRevoked { get; set; }
    
    public bool IsExpired => DateTime.UtcNow >= ExpiresDate;
    public bool IsRevoked => RevokedDate != null;
    public bool IsActive => !IsRevoked && !IsExpired;
    
    // Navigation Properties
    public User User { get; set; } = null!;
}
