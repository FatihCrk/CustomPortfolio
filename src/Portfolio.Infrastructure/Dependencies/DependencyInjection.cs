using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Infrastructure.Services;
using Portfolio.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Portfolio.Infrastructure.Dependencies;

/// <summary>
/// Infrastructure layer dependency injection registrations.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IHealthCheckService, HealthCheckService>();
        services.AddScoped<IRateLimitService, RateLimitService>();
        services.AddScoped<ISlugService, SlugService>();
        
        // Current User Service
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        
        return services;
    }
}

/// <summary>
/// Implementation of ICurrentUserService.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            if (Guid.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            return null;
        }
    }

    public string? Username => _httpContextAccessor.HttpContext?.User?.FindFirst("username")?.Value;

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}
