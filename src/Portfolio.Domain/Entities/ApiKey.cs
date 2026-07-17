using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class ApiKey : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty; // Hashed
    public Guid OwnerId { get; set; }
    public DateTime? ExpiresDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string? LastUsedDate { get; set; }
    public string? Permissions { get; set; } // JSON array of permissions
    
    // Navigation Properties
    public User Owner { get; set; } = null!;
}
