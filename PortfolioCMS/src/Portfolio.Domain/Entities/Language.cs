using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class Language : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // en, tr, etc.
    public string FlagIconUrl { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; } = true;
    public int Order { get; set; }
}
