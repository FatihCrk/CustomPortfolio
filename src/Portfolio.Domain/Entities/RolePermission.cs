using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class RolePermission : BaseEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    
    // Navigation Properties
    public Role Role { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
}
