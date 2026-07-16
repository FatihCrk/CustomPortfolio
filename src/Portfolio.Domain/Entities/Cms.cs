using Portfolio.Domain.Common;
using Portfolio.Domain.Enums;

namespace Portfolio.Domain.Entities;

/// <summary>
/// Site genel ayarlarını temsil eder.
/// </summary>
public class SiteSetting : AuditableEntity
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Group { get; set; }
    public bool IsPublic { get; set; } = false;
}

/// <summary>
/// Profil bilgilerini temsil eder.
/// </summary>
public class Profile : AuditableEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? ShortDescription { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? CvUrl { get; set; }
    public int YearsOfExperience { get; set; }
    public int ProjectsCompleted { get; set; }
    public int ClientsSatisfied { get; set; }
    public int CoffeeCups { get; set; } = 0;
}

/// <summary>
/// Hero (ana sayfa üst bölüm) içeriğini temsil eder.
/// </summary>
public class Hero : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? BackgroundImageUrl { get; set; }
    public string? BackgroundVideoUrl { get; set; }
    public string PrimaryButtonText { get; set; } = string.Empty;
    public string? PrimaryButtonLink { get; set; }
    public string SecondaryButtonText { get; set; } = string.Empty;
    public string? SecondaryButtonLink { get; set; }
    public bool ShowStats { get; set; } = true;
    public int DisplayOrder { get; set; } = 1;
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Yetenekleri temsil eder.
/// </summary>
public class Skill : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; } = 50;
    public string? Category { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Deneyim (iş geçmişi) bilgilerini temsil eder.
/// </summary>
public class Experience : AuditableEntity
{
    public string Company { get; set; } = string.Empty;
    public string? CompanyLogoUrl { get; set; }
    public string Position { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrent { get; set; } = false;
    public string? Description { get; set; }
    public string Location { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } = 0;
}

/// <summary>
/// Eğitim bilgilerini temsil eder.
/// </summary>
public class Education : AuditableEntity
{
    public string Institution { get; set; } = string.Empty;
    public string? InstitutionLogoUrl { get; set; }
    public string Degree { get; set; } = string.Empty;
    public string Field { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public string? Grade { get; set; }
    public int DisplayOrder { get; set; } = 0;
}

/// <summary>
/// Proje kategorilerini temsil eder.
/// </summary>
public class ProjectCategory : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; } = 0;
    
    // Navigation Properties
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}

/// <summary>
/// Projeleri temsil eder.
/// </summary>
public class Project : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public Guid? CategoryId { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public bool IsFeatured { get; set; } = false;
    public int DisplayOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    
    // Navigation Properties
    public virtual ProjectCategory? Category { get; set; }
    public virtual ICollection<ProjectImage> Images { get; set; } = new List<ProjectImage>();
    public virtual ICollection<ProjectTechnology> Technologies { get; set; } = new List<ProjectTechnology>();
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}

/// <summary>
/// Proje görsellerini temsil eder.
/// </summary>
public class ProjectImage : BaseEntity
{
    public Guid ProjectId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int DisplayOrder { get; set; } = 0;
    
    // Navigation Properties
    public virtual Project Project { get; set; } = null!;
}

/// <summary>
/// Proje teknolojilerini temsil eder.
/// </summary>
public class ProjectTechnology : BaseEntity
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
    
    // Navigation Properties
    public virtual Project Project { get; set; } = null!;
}

/// <summary>
/// Sertifikaları temsil eder.
/// </summary>
public class Certificate : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Issuer { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? CredentialId { get; set; }
    public string? CredentialUrl { get; set; }
    public string? CertificateImageUrl { get; set; }
    public string? PdfUrl { get; set; }
    public int DisplayOrder { get; set; } = 0;
}

/// <summary>
/// Referansları temsil eder.
/// </summary>
public class Testimonial : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int Rating { get; set; } = 5;
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; } = 0;
}

/// <summary>
/// Blog kategorilerini temsil eder.
/// </summary>
public class BlogCategory : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; } = 0;
    
    // Navigation Properties
    public virtual ICollection<BlogPost> Posts { get; set; } = new List<BlogPost>();
}

/// <summary>
/// Blog yazılarını temsil eder.
/// </summary>
public class BlogPost : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string? ShortDescription { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid? AuthorId { get; set; }
    public Guid? CategoryId { get; set; }
    public string? CoverImageUrl { get; set; }
    public PublishStatus Status { get; set; } = PublishStatus.Draft;
    public DateTime? PublishedAt { get; set; }
    public int ViewCount { get; set; } = 0;
    public bool AllowComments { get; set; } = true;
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? CanonicalUrl { get; set; }
    public int ReadingTimeMinutes { get; set; } = 0;
    
    // Navigation Properties
    public virtual User? Author { get; set; }
    public virtual BlogCategory? Category { get; set; }
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public virtual ICollection<BlogComment> Comments { get; set; } = new List<BlogComment>();
}

/// <summary>
/// Blog yorumlarını temsil eder.
/// </summary>
public class BlogComment : AuditableEntity
{
    public Guid BlogPostId { get; set; }
    public Guid? UserId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string AuthorEmail { get; set; } = string.Empty;
    public string? AuthorWebsite { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid? ParentCommentId { get; set; }
    public bool IsApproved { get; set; } = false;
    public bool IsSpam { get; set; } = false;
    
    // Navigation Properties
    public virtual BlogPost BlogPost { get; set; } = null!;
    public virtual User? User { get; set; }
    public virtual BlogComment? ParentComment { get; set; }
    public virtual ICollection<BlogComment> Replies { get; set; } = new List<BlogComment>();
}

/// <summary>
/// Hizmetleri temsil eder.
/// </summary>
public class Service : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? ImageUrl { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// İletişim bilgilerini temsil eder.
/// </summary>
public class ContactInfo : AuditableEntity
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Sosyal medya hesaplarını temsil eder.
/// </summary>
public class SocialMedia : AuditableEntity
{
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Username { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// İletişim formu mesajlarını temsil eder.
/// </summary>
public class ContactMessage : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public MessageStatus Status { get; set; } = MessageStatus.New;
    public string IpAddress { get; set; } = string.Empty;
    public string? UserAgent { get; set; }
    public string? DeviceInfo { get; set; }
    public string? BrowserInfo { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime? RepliedAt { get; set; }
    public string? Reply { get; set; }
    public bool IsSpam { get; set; } = false;
}

/// <summary>
/// Medya kütüphanesindeki dosyaları temsil eder.
/// </summary>
public class MediaFile : AuditableEntity
{
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public FileType FileType { get; set; }
    public string MimeType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public int Width { get; set; } = 0;
    public int Height { get; set; } = 0;
    public string? AltText { get; set; }
    public string? Caption { get; set; }
    public string? Folder { get; set; }
    public ICollection<string> Tags { get; set; } = new List<string>();
}

/// <summary>
/// Etiketleri temsil eder.
/// </summary>
public class Tag : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string? Color { get; set; }
    
    // Navigation Properties
    public virtual ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}

/// <summary>
/// Menü öğelerini temsil eder.
/// </summary>
public class MenuItem : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public Guid? ParentId { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public bool IsExternal { get; set; } = false;
    public string? Target { get; set; }
    public ICollection<string> Roles { get; set; } = new List<string>();
    
    // Navigation Properties
    public virtual MenuItem? Parent { get; set; }
    public virtual ICollection<MenuItem> Children { get; set; } = new List<MenuItem>();
}

/// <summary>
/// Sayfa içeriklerini temsil eder (Page Builder için).
/// </summary>
public class Page : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? Template { get; set; }
    public bool IsPublished { get; set; } = false;
    public DateTime? PublishedAt { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public bool IsHomePage { get; set; } = false;
}

/// <summary>
/// Widget tanımlarını temsil eder.
/// </summary>
public class Widget : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Zone { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public string? Settings { get; set; }
}

/// <summary>
/// Tema ayarlarını temsil eder.
/// </summary>
public class Theme : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = false;
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public string? AccentColor { get; set; }
    public string? BackgroundColor { get; set; }
    public string? TextColor { get; set; }
    public string? FontFamily { get; set; }
    public string? Settings { get; set; }
}

/// <summary>
/// Ziyaretçi istatistiklerini temsil eder.
/// </summary>
public class VisitorStat : BaseEntity
{
    public string IpAddress { get; set; } = string.Empty;
    public string? UserAgent { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Referrer { get; set; }
    public string? Page { get; set; }
    public DateTime VisitedAt { get; set; } = DateTime.UtcNow;
    public string? SessionId { get; set; }
}

/// <summary>
/// E-posta şablonlarını temsil eder.
/// </summary>
public class EmailTemplate : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string? Variables { get; set; }
}

/// <summary>
/// Webhook tanımlarını temsil eder.
/// </summary>
public class Webhook : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public ICollection<string> Events { get; set; } = new List<string>();
    public bool IsActive { get; set; } = true;
    public int RetryCount { get; set; } = 3;
    public DateTime? LastTriggeredAt { get; set; }
}

/// <summary>
/// Feature Flag tanımlarını temsil eder.
/// </summary>
public class FeatureFlag : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsEnabled { get; set; } = false;
    public string? Conditions { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

/// <summary>
/// Yedekleme kayıtlarını temsil eder.
/// </summary>
public class Backup : BaseEntity
{
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string BackupType { get; set; } = string.Empty;
    public bool IsSuccess { get; set; } = true;
    public string? ErrorMessage { get; set; }
    public DateTime CompletedAt { get; set; }
}

/// <summary>
/// İçerik versiyonlarını (revizyon) temsil eder.
/// </summary>
public class ContentRevision : BaseEntity
{
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? ChangeSummary { get; set; }
    public Guid RevisedBy { get; set; }
    public DateTime RevisedAt { get; set; } = DateTime.UtcNow;
}
