namespace Portfolio.Domain.Common;

/// <summary>
/// Audit (denetim) özelliklerini içeren temel soyut sınıf.
/// Oluşturan, Güncelleyen, Silen kullanıcı bilgilerini ve soft delete özelliklerini içerir.
/// </summary>
public abstract class AuditableEntity : BaseEntity
{
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
}
