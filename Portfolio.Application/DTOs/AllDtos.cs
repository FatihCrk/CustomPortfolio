namespace Portfolio.Application.DTOs;

// Base DTO
public abstract class BaseDto
{
    public int Id { get; set; }
}

public abstract class AuditableDto : BaseDto
{
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public string? DeletedBy { get; set; }
}

// Auth DTOs
public class LoginDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; }
}

public class RefreshTokenDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserDto User { get; set; } = new();
}

public class UserDto : AuditableDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public bool IsActive { get; set; }
    public bool IsLockedOut { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool TwoFactorEnabled { get; set; }
}

public class SetupAdminDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class SetupResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public UserDto? AdminUser { get; set; }
}

public class ChangePasswordDto
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class ForgotPasswordDto
{
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordDto
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}

// Project DTOs
public class ProjectDto : AuditableDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? ImageUrl { get; set; }
    public List<string>? Images { get; set; }
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public string Category { get; set; } = string.Empty;
    public List<string> Technologies { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public int Order { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; }
}

public class CreateProjectDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? ImageUrl { get; set; }
    public List<string>? Images { get; set; }
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public string Category { get; set; } = string.Empty;
    public List<string> Technologies { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public int Order { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateProjectDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? ImageUrl { get; set; }
    public List<string>? Images { get; set; }
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public string Category { get; set; } = string.Empty;
    public List<string> Technologies { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public int Order { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; }
}

// Message DTOs
public class MessageDto : AuditableDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string Status { get; set; } = "New"; // New, Read, Archived, Replied
    public bool IsRead { get; set; }
    public bool IsArchived { get; set; }
    public string? Reply { get; set; }
    public DateTime? ReadDate { get; set; }
    public DateTime? RepliedDate { get; set; }
}

public class CreateMessageDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

// Skill DTOs
public class SkillDto : AuditableDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Level { get; set; } // 1-100
    public string? Icon { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
}

public class CreateSkillDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Level { get; set; }
    public string? Icon { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateSkillDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Level { get; set; }
    public string? Icon { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
}

// Experience DTOs
public class ExperienceDto : AuditableDto
{
    public string Company { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string Position { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<string>? Achievements { get; set; }
    public int Order { get; set; }
}

public class CreateExperienceDto
{
    public string Company { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string Position { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<string>? Achievements { get; set; }
    public int Order { get; set; }
}

public class UpdateExperienceDto
{
    public string Company { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string Position { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<string>? Achievements { get; set; }
    public int Order { get; set; }
}

// Education DTOs
public class EducationDto : AuditableDto
{
    public string School { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string? Degree { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public string? DiplomaUrl { get; set; }
    public int Order { get; set; }
}

public class CreateEducationDto
{
    public string School { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string? Degree { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public string? DiplomaUrl { get; set; }
    public int Order { get; set; }
}

public class UpdateEducationDto
{
    public string School { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string? Degree { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public string? DiplomaUrl { get; set; }
    public int Order { get; set; }
}

// Blog/Post DTOs
public class PostDto : AuditableDto
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? CoverImageUrl { get; set; }
    public string Category { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public string AuthorId { get; set; } = string.Empty;
    public string? AuthorName { get; set; }
    public bool IsPublished { get; set; }
    public bool IsDraft { get; set; }
    public DateTime? PublishedDate { get; set; }
    public int ViewCount { get; set; }
    
    // SEO
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? CanonicalUrl { get; set; }
}

public class CreatePostDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? CoverImageUrl { get; set; }
    public string Category { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public bool IsPublished { get; set; }
    public bool IsDraft { get; set; } = true;
    
    // SEO
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
}

public class UpdatePostDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? CoverImageUrl { get; set; }
    public string Category { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public bool IsPublished { get; set; }
    public bool IsDraft { get; set; }
    
    // SEO
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
}

// Reference DTOs
public class ReferenceDto : AuditableDto
{
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsActive { get; set; }
}

public class CreateReferenceDto
{
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateReferenceDto
{
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsActive { get; set; }
}

// Certificate DTOs
public class CertificateDto : AuditableDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? FileUrl { get; set; }
    public string? IssuedBy { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? CredentialId { get; set; }
    public string? CredentialUrl { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
}

public class CreateCertificateDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? FileUrl { get; set; }
    public string? IssuedBy { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? CredentialId { get; set; }
    public string? CredentialUrl { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateCertificateDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? FileUrl { get; set; }
    public string? IssuedBy { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? CredentialId { get; set; }
    public string? CredentialUrl { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
}

// Service Item DTOs
public class ServiceItemDto : AuditableDto
{
    public string Title { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsActive { get; set; }
}

public class CreateServiceItemDto
{
    public string Title { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateServiceItemDto
{
    public string Title { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsActive { get; set; }
}

// Contact Info DTO
public class ContactInfoDto
{
    public int Id { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? MapLatitude { get; set; }
    public string? MapLongitude { get; set; }
    public DateTime UpdatedDate { get; set; }
}

public class UpdateContactInfoDto
{
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? MapLatitude { get; set; }
    public string? MapLongitude { get; set; }
}

// Social Media DTOs
public class SocialMediaDto : AuditableDto
{
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
}

public class CreateSocialMediaDto
{
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateSocialMediaDto
{
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
}

// Hero DTO
public class HeroDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string? BackgroundImageUrl { get; set; }
    public string? ButtonText { get; set; }
    public string? ButtonUrl { get; set; }
    public bool ShowStats { get; set; }
    public int StatsYearsExperience { get; set; }
    public int StatsProjectsCompleted { get; set; }
    public int StatsClientsSatisfied { get; set; }
    public int StatsAwardsWon { get; set; }
    public DateTime UpdatedDate { get; set; }
}

public class UpdateHeroDto
{
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string? BackgroundImageUrl { get; set; }
    public string? ButtonText { get; set; }
    public string? ButtonUrl { get; set; }
    public bool ShowStats { get; set; }
    public int StatsYearsExperience { get; set; }
    public int StatsProjectsCompleted { get; set; }
    public int StatsClientsSatisfied { get; set; }
    public int StatsAwardsWon { get; set; }
}

// About DTO
public class AboutDto
{
    public int Id { get; set; }
    public string? PhotoUrl { get; set; }
    public string? CvUrl { get; set; }
    public string Introduction { get; set; } = string.Empty;
    public string? ShortBio { get; set; }
    public DateTime UpdatedDate { get; set; }
}

public class UpdateAboutDto
{
    public string? PhotoUrl { get; set; }
    public string? CvUrl { get; set; }
    public string Introduction { get; set; } = string.Empty;
    public string? ShortBio { get; set; }
}

// Audit Log DTO
public class AuditLogDto
{
    public long Id { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public int? EntityId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}

// Dashboard Stats DTO
public class DashboardStatsDto
{
    public int TotalMessages { get; set; }
    public int NewMessages { get; set; }
    public int TotalProjects { get; set; }
    public int TotalBlogPosts { get; set; }
    public int PublishedPosts { get; set; }
    public int TotalVisitors { get; set; }
    public List<RecentLoginDto> RecentLogins { get; set; } = new();
    public List<MessageDto> RecentMessages { get; set; } = new();
    public List<AuditLogDto> RecentActivities { get; set; } = new();
}

public class RecentLoginDto
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime LoginDate { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
}

// File Upload DTO
public class FileUploadResultDto
{
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
}

// Generic Response
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();
    
    public static ApiResponse<T> Ok(T data, string message = "Operation successful")
        => new() { Success = true, Message = message, Data = data };
    
    public static ApiResponse<T> Fail(string message, List<string>? errors = null)
        => new() { Success = false, Message = message, Errors = errors ?? new List<string>() };
}

public class PagedResponse<T> : ApiResponse<IEnumerable<T>>
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
    
    public static PagedResponse<T> Create(IEnumerable<T> data, int currentPage, int pageSize, int totalCount)
    {
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        return new PagedResponse<T>
        {
            Success = true,
            Message = "Operation successful",
            Data = data,
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}
