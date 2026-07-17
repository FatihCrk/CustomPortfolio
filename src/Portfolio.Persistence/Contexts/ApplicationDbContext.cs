using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Portfolio.Domain.Common;
using Portfolio.Domain.Entities;

namespace Portfolio.Persistence.Contexts;

/// <summary>
/// Uygulama veritabanı context'i.
/// </summary>
public class ApplicationDbContext : DbContext
{
    private readonly ICurrentUserService? _currentUserService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService? currentUserService = null)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    #region DbSets - Identity

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<ApiKey> ApiKeys => Set<ApiKey>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
    public DbSet<TokenBlacklist> TokenBlacklists => Set<TokenBlacklist>();

    #endregion

    #region DbSets - CMS

    public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<Hero> Heroes => Set<Hero>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<Experience> Experiences => Set<Experience>();
    public DbSet<Education> Educations => Set<Education>();
    public DbSet<ProjectCategory> ProjectCategories => Set<ProjectCategory>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectImage> ProjectImages => Set<ProjectImage>();
    public DbSet<ProjectTechnology> ProjectTechnologies => Set<ProjectTechnology>();
    public DbSet<ProjectTag> ProjectTags => Set<ProjectTag>();
    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<Testimonial> Testimonials => Set<Testimonial>();
    public DbSet<BlogCategory> BlogCategories => Set<BlogCategory>();
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<BlogTag> BlogTags => Set<BlogTag>();
    public DbSet<BlogComment> BlogComments => Set<BlogComment>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<ContactInfo> ContactInfos => Set<ContactInfo>();
    public DbSet<SocialMedia> SocialMedias => Set<SocialMedia>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<MediaFile> MediaFiles => Set<MediaFile>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<Page> Pages => Set<Page>();
    public DbSet<Widget> Widgets => Set<Widget>();
    public DbSet<Theme> Themes => Set<Theme>();
    public DbSet<VisitorStat> VisitorStats => Set<VisitorStat>();
    public DbSet<EmailTemplate> EmailTemplates => Set<EmailTemplate>();
    public DbSet<Webhook> Webhooks => Set<Webhook>();
    public DbSet<FeatureFlag> FeatureFlags => Set<FeatureFlag>();
    public DbSet<Backup> Backups => Set<Backup>();
    public DbSet<ContentRevision> ContentRevisions => Set<ContentRevision>();
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<HomePageContent> HomePageContents => Set<HomePageContent>();
    public DbSet<VisitorAnalytics> VisitorAnalytics => Set<VisitorAnalytics>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tüm entity'leri yapılandır
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Soft Delete global query filter
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(AuditableEntity.IsDeleted));
                var condition = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(condition, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService?.UserId?.ToString();
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedBy = _currentUserService?.UserId?.ToString();
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.DeletedBy = _currentUserService?.UserId?.ToString();
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    entry.Entity.IsDeleted = true;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}

/// <summary>
/// Mevcut kullanıcı bilgilerini sağlayan servis arayüzü.
/// </summary>
public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? Username { get; }
    bool IsAuthenticated { get; }
}
