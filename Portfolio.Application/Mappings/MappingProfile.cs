using AutoMapper;
using Portfolio.Application.DTOs;
using Portfolio.Domain.Entities;

namespace Portfolio.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Identity Mappings
        CreateMap<AppUser, UserDto>().ReverseMap();
        CreateMap<AppUser, CreateUserDto>();
        CreateMap<AppUser, UpdateUserDto>();
        
        // Project Mappings
        CreateMap<Project, ProjectDto>()
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name));
        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            
        // Category Mappings
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();
        
        // Skill Mappings
        CreateMap<Skill, SkillDto>()
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name));
        CreateMap<CreateSkillDto, Skill>();
        CreateMap<UpdateSkillDto, Skill>();
        
        // Experience Mappings
        CreateMap<Experience, ExperienceDto>().ReverseMap();
        CreateMap<CreateExperienceDto, Experience>();
        CreateMap<UpdateExperienceDto, Experience>();
        
        // Education Mappings
        CreateMap<Education, EducationDto>().ReverseMap();
        CreateMap<CreateEducationDto, Education>();
        CreateMap<UpdateEducationDto, Education>();
        
        // Certificate Mappings
        CreateMap<Certificate, CertificateDto>().ReverseMap();
        CreateMap<CreateCertificateDto, Certificate>();
        CreateMap<UpdateCertificateDto, Certificate>();
        
        // Reference Mappings
        CreateMap<Reference, ReferenceDto>().ReverseMap();
        CreateMap<CreateReferenceDto, Reference>();
        CreateMap<UpdateReferenceDto, Reference>();
        
        // Service Mappings
        CreateMap<ServiceItem, ServiceItemDto>().ReverseMap();
        CreateMap<CreateServiceItemDto, ServiceItem>();
        CreateMap<UpdateServiceItemDto, ServiceItem>();
        
        // Blog Mappings
        CreateMap<BlogPost, BlogPostDto>()
            .ForMember(d => d.AuthorName, opt => opt.MapFrom(s => s.Author.FullName))
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name));
        CreateMap<CreateBlogPostDto, BlogPost>();
        CreateMap<UpdateBlogPostDto, BlogPost>();
        
        // Message Mappings
        CreateMap<Message, MessageDto>().ReverseMap();
        CreateMap<CreateMessageDto, Message>();
        
        // Contact Info Mappings
        CreateMap<ContactInfo, ContactInfoDto>().ReverseMap();
        CreateMap<CreateContactInfoDto, ContactInfo>();
        CreateMap<UpdateContactInfoDto, ContactInfo>();
        
        // Social Media Mappings
        CreateMap<SocialMedia, SocialMediaDto>().ReverseMap();
        CreateMap<CreateSocialMediaDto, SocialMedia>();
        CreateMap<UpdateSocialMediaDto, SocialMedia>();
        
        // Hero Section Mappings
        CreateMap<HeroSection, HeroSectionDto>().ReverseMap();
        CreateMap<CreateHeroSectionDto, HeroSection>();
        CreateMap<UpdateHeroSectionDto, HeroSection>();
        
        // About Section Mappings
        CreateMap<AboutSection, AboutSectionDto>().ReverseMap();
        CreateMap<CreateAboutSectionDto, AboutSection>();
        CreateMap<UpdateAboutSectionDto, AboutSection>();
        
        // Statistic Mappings
        CreateMap<Statistic, StatisticDto>().ReverseMap();
        CreateMap<CreateStatisticDto, Statistic>();
        CreateMap<UpdateStatisticDto, Statistic>();
        
        // Media File Mappings
        CreateMap<MediaFile, MediaFileDto>().ReverseMap();
        
        // Audit Log Mappings
        CreateMap<AuditLog, AuditLogDto>().ReverseMap();
        
        // Site Settings Mappings
        CreateMap<SiteSetting, SiteSettingDto>().ReverseMap();
        CreateMap<UpdateSiteSettingDto, SiteSetting>();
    }
}
