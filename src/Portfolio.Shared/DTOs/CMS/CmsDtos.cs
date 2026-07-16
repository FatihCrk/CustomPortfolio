namespace Portfolio.Shared.DTOs.CMS;

// Base DTOs
public abstract class BaseDto
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; } = true;
}

// Hero Section
public class HeroSectionDto : BaseDto
{
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string ButtonText { get; set; } = string.Empty;
    public string ButtonUrl { get; set; } = string.Empty;
    public string BackgroundImageUrl { get; set; } = string.Empty;
    public int Order { get; set; }
}

public class CreateHeroSectionDto
{
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string ButtonText { get; set; } = string.Empty;
    public string ButtonUrl { get; set; } = string.Empty;
    public string BackgroundImageUrl { get; set; } = string.Empty;
    public int Order { get; set; }
}

public class UpdateHeroSectionDto
{
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public string? ButtonText { get; set; }
    public string? ButtonUrl { get; set; }
    public string? BackgroundImageUrl { get; set; }
    public int? Order { get; set; }
}

// About
public class AboutDto : BaseDto
{
    public string FullName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string CvUrl { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public int ProjectsCompleted { get; set; }
}

public class CreateAboutDto
{
    public string FullName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string CvUrl { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public int ProjectsCompleted { get; set; }
}

public class UpdateAboutDto
{
    public string? FullName { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? CvUrl { get; set; }
    public int? YearsOfExperience { get; set; }
    public int? ProjectsCompleted { get; set; }
}

// Skill
public class SkillDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Level { get; set; } // 1-100
    public string? IconUrl { get; set; }
    public int Order { get; set; }
}

public class CreateSkillDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Level { get; set; }
    public string? IconUrl { get; set; }
    public int Order { get; set; }
}

public class UpdateSkillDto
{
    public string? Name { get; set; }
    public string? Category { get; set; }
    public int? Level { get; set; }
    public string? IconUrl { get; set; }
    public int? Order { get; set; }
}

// Experience
public class ExperienceDto : BaseDto
{
    public string CompanyName { get; set; } = string.Empty;
    public string? CompanyLogoUrl { get; set; }
    public string Position { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsCurrent { get; set; }
    public int Order { get; set; }
}

public class CreateExperienceDto
{
    public string CompanyName { get; set; } = string.Empty;
    public string? CompanyLogoUrl { get; set; }
    public string Position { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsCurrent { get; set; }
    public int Order { get; set; }
}

public class UpdateExperienceDto
{
    public string? CompanyName { get; set; }
    public string? CompanyLogoUrl { get; set; }
    public string? Position { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public bool? IsCurrent { get; set; }
    public int? Order { get; set; }
}

// Education
public class EducationDto : BaseDto
{
    public string SchoolName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? DiplomaUrl { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; }
}

public class CreateEducationDto
{
    public string SchoolName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? DiplomaUrl { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; }
}

public class UpdateEducationDto
{
    public string? SchoolName { get; set; }
    public string? Department { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? DiplomaUrl { get; set; }
    public string? Description { get; set; }
    public int? Order { get; set; }
}

// Project
public class ProjectDto : BaseDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public List<string> ImageUrls { get; set; } = new();
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public List<string> Technologies { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public string Category { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public int Order { get; set; }
}

public class CreateProjectDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public List<string> ImageUrls { get; set; } = new();
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public List<string> Technologies { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public string Category { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public int Order { get; set; }
}

public class UpdateProjectDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public List<string>? ImageUrls { get; set; }
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public List<string>? Technologies { get; set; }
    public List<string>? Tags { get; set; }
    public string? Category { get; set; }
    public bool? IsFeatured { get; set; }
    public int? Order { get; set; }
}

// Certificate
public class CertificateDto : BaseDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? FileUrl { get; set; }
    public string IssuedBy { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public int Order { get; set; }
}

public class CreateCertificateDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? FileUrl { get; set; }
    public string IssuedBy { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public int Order { get; set; }
}

public class UpdateCertificateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? FileUrl { get; set; }
    public string? IssuedBy { get; set; }
    public DateTime? IssueDate { get; set; }
    public int? Order { get; set; }
}

// Testimonial
public class TestimonialDto : BaseDto
{
    public string ClientName { get; set; } = string.Empty;
    public string ClientTitle { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string? ClientPhotoUrl { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int Rating { get; set; } // 1-5
    public int Order { get; set; }
}

public class CreateTestimonialDto
{
    public string ClientName { get; set; } = string.Empty;
    public string ClientTitle { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string? ClientPhotoUrl { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int Rating { get; set; }
    public int Order { get; set; }
}

public class UpdateTestimonialDto
{
    public string? ClientName { get; set; }
    public string? ClientTitle { get; set; }
    public string? Company { get; set; }
    public string? ClientPhotoUrl { get; set; }
    public string? Comment { get; set; }
    public int? Rating { get; set; }
    public int? Order { get; set; }
}

// Blog Post
public class BlogPostDto : BaseDto
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string Category { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string AuthorId { get; set; } = string.Empty;
    public string? AuthorName { get; set; }
    public DateTime PublishDate { get; set; }
    public bool IsPublished { get; set; }
    public bool IsDraft { get; set; }
    public int ViewCount { get; set; }
}

public class CreateBlogPostDto
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string Category { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string AuthorId { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
    public bool IsPublished { get; set; }
    public bool IsDraft { get; set; }
}

public class UpdateBlogPostDto
{
    public string? Title { get; set; }
    public string? Slug { get; set; }
    public string? Content { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Category { get; set; }
    public List<string>? Tags { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public DateTime? PublishDate { get; set; }
    public bool? IsPublished { get; set; }
    public bool? IsDraft { get; set; }
}

// Service
public class ServiceDto : BaseDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public int Order { get; set; }
}

public class CreateServiceDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public int Order { get; set; }
}

public class UpdateServiceDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public int? Order { get; set; }
}

// Contact Message
public class ContactMessageDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string MessageBody { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string? UserAgent { get; set; }
    public bool IsRead { get; set; }
    public bool IsArchived { get; set; }
    public string? AdminResponse { get; set; }
    public DateTime? RespondedDate { get; set; }
}

public class CreateContactMessageDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string MessageBody { get; set; } = string.Empty;
}

public class UpdateContactMessageDto
{
    public bool? IsRead { get; set; }
    public bool? IsArchived { get; set; }
    public string? AdminResponse { get; set; }
}

// Social Media
public class SocialMediaDto : BaseDto
{
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateSocialMediaDto
{
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public int Order { get; set; }
}

public class UpdateSocialMediaDto
{
    public string? Platform { get; set; }
    public string? Url { get; set; }
    public string? IconUrl { get; set; }
    public int? Order { get; set; }
    public bool? IsActive { get; set; }
}

// Site Settings
public class SiteSettingsDto : BaseDto
{
    public string SiteName { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string? FaviconUrl { get; set; }
    public string FooterText { get; set; } = string.Empty;
    public string CopyrightText { get; set; } = string.Empty;
    public string PrimaryColor { get; set; } = "#3b82f6";
    public string SecondaryColor { get; set; } = "#1e40af";
    public string FontFamily { get; set; } = "Inter";
    public bool IsMaintenanceMode { get; set; }
    public string? GoogleAnalyticsId { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
}

public class CreateSiteSettingsDto
{
    public string SiteName { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string? FaviconUrl { get; set; }
    public string FooterText { get; set; } = string.Empty;
    public string CopyrightText { get; set; } = string.Empty;
    public string PrimaryColor { get; set; } = "#3b82f6";
    public string SecondaryColor { get; set; } = "#1e40af";
    public string FontFamily { get; set; } = "Inter";
    public bool IsMaintenanceMode { get; set; }
    public string? GoogleAnalyticsId { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
}

public class UpdateSiteSettingsDto
{
    public string? SiteName { get; set; }
    public string? LogoUrl { get; set; }
    public string? FaviconUrl { get; set; }
    public string? FooterText { get; set; }
    public string? CopyrightText { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public string? FontFamily { get; set; }
    public bool? IsMaintenanceMode { get; set; }
    public string? GoogleAnalyticsId { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
}

// Generic Response for Pagination
public class PagedResponse<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
