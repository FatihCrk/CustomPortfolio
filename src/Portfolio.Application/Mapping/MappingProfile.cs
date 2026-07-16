using AutoMapper;
using Portfolio.Domain.Entities.CMS;
using Portfolio.Domain.Entities.Identity;
using Portfolio.Shared.DTOs.Auth;
using Portfolio.Shared.DTOs.CMS;

namespace Portfolio.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Identity Mappings
        CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles handled separately

        // CMS Mappings - Examples (Expand for all entities)
        CreateMap<HeroSection, HeroSectionDto>().ReverseMap();
        CreateMap<About, AboutDto>().ReverseMap();
        CreateMap<Skill, SkillDto>().ReverseMap();
        CreateMap<Experience, ExperienceDto>().ReverseMap();
        CreateMap<Education, EducationDto>().ReverseMap();
        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<Certificate, CertificateDto>().ReverseMap();
        CreateMap<Testimonial, TestimonialDto>().ReverseMap();
        CreateMap<BlogPost, BlogPostDto>().ReverseMap();
        CreateMap<Service, ServiceDto>().ReverseMap();
        CreateMap<ContactMessage, ContactMessageDto>().ReverseMap();
        CreateMap<SocialMedia, SocialMediaDto>().ReverseMap();
        CreateMap<SiteSettings, SiteSettingsDto>().ReverseMap();
        
        // Create -> CreateDto
        CreateMap<CreateHeroSectionDto, HeroSection>();
        CreateMap<CreateAboutDto, About>();
        CreateMap<CreateSkillDto, Skill>();
        CreateMap<CreateExperienceDto, Experience>();
        CreateMap<CreateEducationDto, Education>();
        CreateMap<CreateProjectDto, Project>();
        CreateMap<CreateCertificateDto, Certificate>();
        CreateMap<CreateTestimonialDto, Testimonial>();
        CreateMap<CreateBlogPostDto, BlogPost>();
        CreateMap<CreateServiceDto, Service>();
        CreateMap<CreateContactMessageDto, ContactMessage>();
        CreateMap<CreateSocialMediaDto, SocialMedia>();
        CreateMap<CreateSiteSettingsDto, SiteSettings>();

        // Update -> Entity
        CreateMap<UpdateHeroSectionDto, HeroSection>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UpdateAboutDto, About>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UpdateSkillDto, Skill>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UpdateExperienceDto, Experience>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UpdateEducationDto, Education>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UpdateProjectDto, Project>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UpdateCertificateDto, Certificate>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UpdateTestimonialDto, Testimonial>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UpdateBlogPostDto, BlogPost>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UpdateServiceDto, Service>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UpdateSiteSettingsDto, SiteSettings>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
