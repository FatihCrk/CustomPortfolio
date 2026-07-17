using Portfolio.Application.DTOs;
using Portfolio.Domain.Entities;
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
        
        // Add more mappings here as entities are created
        // CreateMap<SourceDto, DestinationEntity>();
        // CreateMap<DestinationEntity, SourceDto>();
    }
}
