using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class Translation : BaseEntity
{
    public string Key { get; set; } = string.Empty;
    public string LanguageCode { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Module { get; set; } // To group translations by module
}
