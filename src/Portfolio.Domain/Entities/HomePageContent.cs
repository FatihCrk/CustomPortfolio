using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class HomePageContent : BaseEntity
{
    // Hero Section
    public string? HeroTitle { get; set; }
    public string? HeroSubtitle { get; set; }
    public string? HeroButtonText { get; set; }
    public string? HeroButtonUrl { get; set; }
    public Guid? HeroBackgroundImageId { get; set; }
    
    // About Section
    public string? AboutTitle { get; set; }
    public string? AboutDescription { get; set; }
    public Guid? AboutPhotoId { get; set; }
    public string? CvUrl { get; set; }
    
    // Statistics
    public int TotalProjects { get; set; }
    public int TotalClients { get; set; }
    public int TotalExperienceYears { get; set; }
    public int TotalAwards { get; set; }
    
    // Navigation Properties
    public MediaFile? HeroBackgroundImage { get; set; }
    public MediaFile? AboutPhoto { get; set; }
}
