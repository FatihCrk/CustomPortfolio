using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Persistence.Contexts;

namespace Portfolio.Persistence.Dependencies;

/// <summary>
/// Persistence layer dependency injection registrations.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database Configuration (Supports both PostgreSQL and SQL Server)
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        }

        // Auto-detect database type from connection string
        if (connectionString.Contains("Host=") || connectionString.Contains("PostgreSQL"))
        {
            // PostgreSQL
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString, b =>
                {
                    b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    b.EnableRetryOnFailure(3, TimeSpan.FromSeconds(30), null);
                }));
        }
        else
        {
            // SQL Server (default)
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, b =>
                {
                    b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    b.EnableRetryOnFailure(3, TimeSpan.FromSeconds(30), null);
                }));
        }

        return services;
    }
}
