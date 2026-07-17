using Portfolio.Application.DTOs;
using Portfolio.Domain.Entities;
using Portfolio.Domain.Entities.Identity;
using Portfolio.Application.Features.Users.Queries.GetUsers;
using Portfolio.Application.Features.Users.Commands.CreateUser;
using AutoMapper;

namespace Portfolio.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Setup Wizard
        CreateMap<SetupWizardDto, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));
        
        CreateMap<Role, RoleDto>();

        // Project mappings
        CreateMap<Project, ProjectDto>();
        CreateMap<CreateProjectCommand, Project>();
        CreateMap<UpdateProjectCommand, Project>();

        // Blog mappings
        CreateMap<BlogPost, BlogPostDto>();
        CreateMap<CreateBlogPostCommand, BlogPost>();
        CreateMap<UpdateBlogPostCommand, BlogPost>();

        // Skill mappings
        CreateMap<Skill, SkillDto>();
        CreateMap<CreateSkillCommand, Skill>();
        CreateMap<UpdateSkillCommand, Skill>();

        // Experience mappings
        CreateMap<Experience, ExperienceDto>();
        CreateMap<CreateExperienceCommand, Experience>();
        CreateMap<UpdateExperienceCommand, Experience>();

        // Education mappings
        CreateMap<Education, EducationDto>();
        CreateMap<CreateEducationCommand, Education>();
        CreateMap<UpdateEducationCommand, Education>();

        // Certificate mappings
        CreateMap<Certificate, CertificateDto>();
        CreateMap<CreateCertificateCommand, Certificate>();
        CreateMap<UpdateCertificateCommand, Certificate>();

        // Testimonial mappings
        CreateMap<Testimonial, TestimonialDto>();
        CreateMap<CreateTestimonialCommand, Testimonial>();
        CreateMap<UpdateTestimonialCommand, Testimonial>();

        // Service mappings
        CreateMap<Service, ServiceDto>();
        CreateMap<CreateServiceCommand, Service>();
        CreateMap<UpdateServiceCommand, Service>();

        // Contact Message mappings
        CreateMap<ContactMessage, ContactMessageDto>();

        // Media mappings
        CreateMap<MediaFile, MediaFileDto>();

        // Site Settings mappings
        CreateMap<SiteSetting, SiteSettingDto>();
        CreateMap<UpdateSiteSettingCommand, SiteSetting>();

        // Theme mappings
        CreateMap<Theme, ThemeDto>();
        CreateMap<UpdateThemeCommand, Theme>();
    }
}

// DTOs for various entities
public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
}

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public bool IsFeatured { get; set; }
    public int Order { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class CreateProjectCommand
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public bool IsFeatured { get; set; }
    public int Order { get; set; }
}

public class UpdateProjectCommand
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string? DemoUrl { get; set; }
    public bool IsFeatured { get; set; }
    public int Order { get; set; }
}

public class BlogPostDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string Slug { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public DateTime PublishedDate { get; set; }
}

public class CreateBlogPostCommand
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string Slug { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
}

public class UpdateBlogPostCommand
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string Slug { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
}

public class SkillDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int Order { get; set; }
}

public class CreateSkillCommand
{
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int Order { get; set; }
}

public class UpdateSkillCommand
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int Order { get; set; }
}

public class ExperienceDto
{
    public Guid Id { get; set; }
    public string Company { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsCurrent { get; set; }
}

public class CreateExperienceCommand
{
    public string Company { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsCurrent { get; set; }
}

public class UpdateExperienceCommand
{
    public Guid Id { get; set; }
    public string Company { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsCurrent { get; set; }
}

public class EducationDto
{
    public Guid Id { get; set; }
    public string School { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string Field { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
}

public class CreateEducationCommand
{
    public string School { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string Field { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
}

public class UpdateEducationCommand
{
    public Guid Id { get; set; }
    public string School { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string Field { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
}

public class CertificateDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Issuer { get; set; }
    public DateTime IssueDate { get; set; }
    public string? CredentialUrl { get; set; }
    public string? ImageUrl { get; set; }
}

public class CreateCertificateCommand
{
    public string Title { get; set; } = string.Empty;
    public string? Issuer { get; set; }
    public DateTime IssueDate { get; set; }
    public string? CredentialUrl { get; set; }
    public string? ImageUrl { get; set; }
}

public class UpdateCertificateCommand
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Issuer { get; set; }
    public DateTime IssueDate { get; set; }
    public string? CredentialUrl { get; set; }
    public string? ImageUrl { get; set; }
}

public class TestimonialDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string Comment { get; set; } = string.Empty;
}

public class CreateTestimonialCommand
{
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string Comment { get; set; } = string.Empty;
}

public class UpdateTestimonialCommand
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string Comment { get; set; } = string.Empty;
}

public class ServiceDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int Order { get; set; }
}

public class CreateServiceCommand
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int Order { get; set; }
}

public class UpdateServiceCommand
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int Order { get; set; }
}

public class ContactMessageDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class MediaFileDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? ThumbnailPath { get; set; }
    public DateTime UploadedDate { get; set; }
}

public class SiteSettingDto
{
    public Guid Id { get; set; }
    public string SiteName { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string? FaviconUrl { get; set; }
    public string FooterText { get; set; } = string.Empty;
    public string CopyrightText { get; set; } = string.Empty;
    public bool IsMaintenanceMode { get; set; }
}

public class UpdateSiteSettingCommand
{
    public Guid Id { get; set; }
    public string SiteName { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string? FaviconUrl { get; set; }
    public string FooterText { get; set; } = string.Empty;
    public string CopyrightText { get; set; } = string.Empty;
    public bool IsMaintenanceMode { get; set; }
}

public class ThemeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PrimaryColor { get; set; } = string.Empty;
    public string SecondaryColor { get; set; } = string.Empty;
    public string BackgroundColor { get; set; } = string.Empty;
    public string TextColor { get; set; } = string.Empty;
    public bool IsDark { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateThemeCommand
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PrimaryColor { get; set; } = string.Empty;
    public string SecondaryColor { get; set; } = string.Empty;
    public string BackgroundColor { get; set; } = string.Empty;
    public string TextColor { get; set; } = string.Empty;
    public bool IsDark { get; set; }
    public bool IsActive { get; set; }
}
