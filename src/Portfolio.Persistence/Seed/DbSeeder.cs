using Microsoft.EntityFrameworkCore;
using Portfolio.Persistence.Contexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Persistence.Seed;

/// <summary>
/// Veritabanı seed işlemlerini yönetir.
/// </summary>
public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        // Check if any user exists
        if (!context.Users.Any())
        {
            await SeedDefaultData(context);
        }
        
        // Check if setup wizard is completed
        var setupCompleted = context.SiteSettings
            .Any(x => x.Key == "SetupCompleted" && x.Value == "true");
            
        if (!setupCompleted)
        {
            // Setup wizard not completed yet, system will show setup page
            await SeedInitialSettings(context);
        }
    }

    private static async Task SeedDefaultData(ApplicationDbContext context)
    {
        // Default Roles
        var roles = new List<Role>
        {
            new() { Id = Guid.NewGuid(), Name = "SuperAdmin", Description = "System Administrator", IsSystemRole = true },
            new() { Id = Guid.NewGuid(), Name = "Admin", Description = "Administrator", IsSystemRole = true },
            new() { Id = Guid.NewGuid(), Name = "Editor", Description = "Content Editor", IsSystemRole = true },
            new() { Id = Guid.NewGuid(), Name = "Viewer", Description = "Read Only User", IsSystemRole = true }
        };
        
        await context.Roles.AddRangeAsync(roles);
        
        // Default Permissions
        var permissions = new List<Permission>
        {
            new() { Id = Guid.NewGuid(), Name = "View Dashboard", Resource = "Dashboard", Action = "View" },
            new() { Id = Guid.NewGuid(), Name = "Manage Users", Resource = "Users", Action = "Manage" },
            new() { Id = Guid.NewGuid(), Name = "Manage Content", Resource = "Content", Action = "Manage" },
            new() { Id = Guid.NewGuid(), Name = "Manage Settings", Resource = "Settings", Action = "Manage" }
        };
        
        await context.Permissions.AddRangeAsync(permissions);
        
        // Default Site Settings
        var settings = new List<SiteSetting>
        {
            new() { Id = Guid.NewGuid(), Key = "SiteName", Value = "Portfolio CMS", Type = "string", Group = "General" },
            new() { Id = Guid.NewGuid(), Key = "SetupCompleted", Value = "false", Type = "boolean", Group = "System" },
            new() { Id = Guid.NewGuid(), Key = "MaintenanceMode", Value = "false", Type = "boolean", Group = "System" },
            new() { Id = Guid.NewGuid(), Key = "DefaultLanguage", Value = "tr", Type = "string", Group = "Localization" },
            new() { Id = Guid.NewGuid(), Key = "SupportedLanguages", Value = "tr,en", Type = "array", Group = "Localization" }
        };
        
        await context.SiteSettings.AddRangeAsync(settings);
        
        await context.SaveChangesAsync();
    }

    private static async Task SeedInitialSettings(ApplicationDbContext context)
    {
        // Ensure initial settings exist
        if (!context.SiteSettings.Any(x => x.Key == "SetupCompleted"))
        {
            var setting = new SiteSetting
            {
                Id = Guid.NewGuid(),
                Key = "SetupCompleted",
                Value = "false",
                Type = "boolean",
                Group = "System",
                Description = "Indicates if the initial setup wizard has been completed"
            };
            
            context.SiteSettings.Add(setting);
            await context.SaveChangesAsync();
        }
    }
}
