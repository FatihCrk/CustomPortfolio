namespace Portfolio.Domain.Common;

/// <summary>
/// Tüm veri tabanı varlıkları için temel soyut sınıf.
/// Primary Key ve ortak özellikleri içerir.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
