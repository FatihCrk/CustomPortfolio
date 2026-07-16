using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class Theme : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Color settings stored as JSON
    public string PrimaryColor { get; set; } = "#007bff";
    public string SecondaryColor { get; set; } = "#6c757d";
    public string BackgroundColor { get; set; } = "#ffffff";
    public string TextColor { get; set; } = "#212529";
    public string? CustomCss { get; set; }
}
