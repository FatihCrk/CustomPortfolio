using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class Theme : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = false;
    public string PrimaryColor { get; set; } = "#007bff";
    public string SecondaryColor { get; set; } = "#6c757d";
    public string AccentColor { get; set; } = "#17a2b8";
    public string BackgroundColor { get; set; } = "#ffffff";
    public string TextColor { get; set; } = "#212529";
    public string FontFamily { get; set; } = "Inter";
    public string? CssOverrides { get; set; } // Custom CSS
}
