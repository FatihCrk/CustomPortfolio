using System;

namespace Portfolio.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
    
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public Guid? DeletedBy { get; set; }
    
    public bool IsDeleted { get; set; }
    public int Version { get; set; } = 1; // Optimistic Concurrency
}
