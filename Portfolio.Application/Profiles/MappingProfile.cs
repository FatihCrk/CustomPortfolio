using AutoMapper;
using Portfolio.Application.DTOs;
using Portfolio.Domain.Entities;

namespace Portfolio.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<AppUser, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        // Project mappings
        CreateMap<Project, ProjectDto>();
        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>();

        // Message mappings
        CreateMap<Message, MessageDto>();
        CreateMap<CreateMessageDto, Message>();

        // Skill mappings
        CreateMap<Skill, SkillDto>();
        CreateMap<CreateSkillDto, Skill>();
        CreateMap<UpdateSkillDto, Skill>();

        // Experience mappings
        CreateMap<Experience, ExperienceDto>();
        CreateMap<CreateExperienceDto, Experience>();
        CreateMap<UpdateExperienceDto, Experience>();

        // Education mappings
        CreateMap<Education, EducationDto>();
        CreateMap<CreateEducationDto, Education>();
        CreateMap<UpdateEducationDto, Education>();

        // Post/Blog mappings
        CreateMap<Post, PostDto>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.UserName));
        CreateMap<CreatePostDto, Post>();
        CreateMap<UpdatePostDto, Post>();

        // Reference mappings
        CreateMap<Reference, ReferenceDto>();
        CreateMap<CreateReferenceDto, Reference>();
        CreateMap<UpdateReferenceDto, Reference>();

        // Certificate mappings
        CreateMap<Certificate, CertificateDto>();
        CreateMap<CreateCertificateDto, Certificate>();
        CreateMap<UpdateCertificateDto, Certificate>();

        // ServiceItem mappings
        CreateMap<ServiceItem, ServiceItemDto>();
        CreateMap<CreateServiceItemDto, ServiceItem>();
        CreateMap<UpdateServiceItemDto, ServiceItem>();

        // ContactInfo mappings
        CreateMap<ContactInfo, ContactInfoDto>();
        CreateMap<UpdateContactInfoDto, ContactInfo>();

        // SocialMedia mappings
        CreateMap<SocialMedia, SocialMediaDto>();
        CreateMap<CreateSocialMediaDto, SocialMedia>();
        CreateMap<UpdateSocialMediaDto, SocialMedia>();

        // Hero mappings
        CreateMap<HeroSection, HeroDto>();
        CreateMap<UpdateHeroDto, HeroSection>();

        // About mappings
        CreateMap<AboutSection, AboutDto>();
        CreateMap<UpdateAboutDto, AboutSection>();

        // AuditLog mappings
        CreateMap<AuditLog, AuditLogDto>();
    }
}
