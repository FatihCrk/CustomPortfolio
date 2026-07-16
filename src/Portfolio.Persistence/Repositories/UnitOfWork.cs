using Portfolio.Domain.Entities;
using Portfolio.Domain.Interfaces;
using Portfolio.Persistence.Contexts;

namespace Portfolio.Persistence.Repositories;

/// <summary>
/// Unit of Work pattern implementasyonu.
/// Transaction yönetimi ve repository koordinasyonu sağlar.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private bool _disposed;

    private IRepository<User>? _users;
    private IRepository<Role>? _roles;
    private IRepository<Permission>? _permissions;
    private IRepository<RefreshToken>? _refreshTokens;
    private IRepository<ActivityLog>? _activityLogs;
    private IRepository<Profile>? _profiles;
    private IRepository<Hero>? _heroes;
    private IRepository<Skill>? _skills;
    private IRepository<Experience>? _experiences;
    private IRepository<Education>? _educations;
    private IRepository<Project>? _projects;
    private IRepository<ProjectCategory>? _projectCategories;
    private IRepository<Certificate>? _certificates;
    private IRepository<Testimonial>? _testimonials;
    private IRepository<BlogPost>? _blogPosts;
    private IRepository<BlogCategory>? _blogCategories;
    private IRepository<Service>? _services;
    private IRepository<ContactMessage>? _contactMessages;
    private IRepository<MediaFile>? _mediaFiles;
    private IRepository<Tag>? _tags;
    private IRepository<MenuItem>? _menuItems;
    private IRepository<Page>? _pages;
    private IRepository<Widget>? _widgets;
    private IRepository<Theme>? _themes;
    private IRepository<SiteSetting>? _siteSettings;
    private IRepository<SocialMedia>? _socialMedias;
    private IRepository<ContactInfo>? _contactInfos;
    private IRepository<VisitorStat>? _visitorStats;
    private IRepository<EmailTemplate>? _emailTemplates;
    private IRepository<Webhook>? _webhooks;
    private IRepository<FeatureFlag>? _featureFlags;
    private IRepository<Backup>? _backups;
    private IRepository<ContentRevision>? _contentRevisions;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IRepository<User> Users => _users ??= new Repository<User>(_context);
    public IRepository<Role> Roles => _roles ??= new Repository<Role>(_context);
    public IRepository<Permission> Permissions => _permissions ??= new Repository<Permission>(_context);
    public IRepository<RefreshToken> RefreshTokens => _refreshTokens ??= new Repository<RefreshToken>(_context);
    public IRepository<ActivityLog> ActivityLogs => _activityLogs ??= new Repository<ActivityLog>(_context);
    public IRepository<Profile> Profiles => _profiles ??= new Repository<Profile>(_context);
    public IRepository<Hero> Heroes => _heroes ??= new Repository<Hero>(_context);
    public IRepository<Skill> Skills => _skills ??= new Repository<Skill>(_context);
    public IRepository<Experience> Experiences => _experiences ??= new Repository<Experience>(_context);
    public IRepository<Education> Educations => _educations ??= new Repository<Education>(_context);
    public IRepository<Project> Projects => _projects ??= new Repository<Project>(_context);
    public IRepository<ProjectCategory> ProjectCategories => _projectCategories ??= new Repository<ProjectCategory>(_context);
    public IRepository<Certificate> Certificates => _certificates ??= new Repository<Certificate>(_context);
    public IRepository<Testimonial> Testimonials => _testimonials ??= new Repository<Testimonial>(_context);
    public IRepository<BlogPost> BlogPosts => _blogPosts ??= new Repository<BlogPost>(_context);
    public IRepository<BlogCategory> BlogCategories => _blogCategories ??= new Repository<BlogCategory>(_context);
    public IRepository<Service> Services => _services ??= new Repository<Service>(_context);
    public IRepository<ContactMessage> ContactMessages => _contactMessages ??= new Repository<ContactMessage>(_context);
    public IRepository<MediaFile> MediaFiles => _mediaFiles ??= new Repository<MediaFile>(_context);
    public IRepository<Tag> Tags => _tags ??= new Repository<Tag>(_context);
    public IRepository<MenuItem> MenuItems => _menuItems ??= new Repository<MenuItem>(_context);
    public IRepository<Page> Pages => _pages ??= new Repository<Page>(_context);
    public IRepository<Widget> Widgets => _widgets ??= new Repository<Widget>(_context);
    public IRepository<Theme> Themes => _themes ??= new Repository<Theme>(_context);
    public IRepository<SiteSetting> SiteSettings => _siteSettings ??= new Repository<SiteSetting>(_context);
    public IRepository<SocialMedia> SocialMedias => _socialMedias ??= new Repository<SocialMedia>(_context);
    public IRepository<ContactInfo> ContactInfos => _contactInfos ??= new Repository<ContactInfo>(_context);
    public IRepository<VisitorStat> VisitorStats => _visitorStats ??= new Repository<VisitorStat>(_context);
    public IRepository<EmailTemplate> EmailTemplates => _emailTemplates ??= new Repository<EmailTemplate>(_context);
    public IRepository<Webhook> Webhooks => _webhooks ??= new Repository<Webhook>(_context);
    public IRepository<FeatureFlag> FeatureFlags => _featureFlags ??= new Repository<FeatureFlag>(_context);
    public IRepository<Backup> Backups => _backups ??= new Repository<Backup>(_context);
    public IRepository<ContentRevision> ContentRevisions => _contentRevisions ??= new Repository<ContentRevision>(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.RollbackTransactionAsync(cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }
        _disposed = true;
    }
}
