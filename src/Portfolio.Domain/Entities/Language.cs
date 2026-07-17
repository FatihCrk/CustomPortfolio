using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class Language : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // en, tr, de
    public bool IsDefault { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; } = 0;
}
