using Microsoft.Extensions.DependencyInjection;
using Portfolio.Application.Mappings;

namespace Portfolio.Application.Dependencies;

/// <summary>
/// Application layer dependency injection registrations.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        
        // Validators (FluentValidation)
        // services.AddValidatorsFromAssemblyContaining<DependencyInjection>();
        
        // MediatR
        // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        
        return services;
    }
}
