using Portfolio.Domain.Entities;

namespace Portfolio.Application.Interfaces;

/// <summary>
/// Unit of Work pattern arayüzü.
/// Transaction yönetimi ve repository'lerin koordinasyonu için kullanılır.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Kullanıcı repository'sine erişim sağlar.
    /// </summary>
    IRepository<User> Users { get; }

    /// <summary>
    /// Rol repository'sine erişim sağlar.
    /// </summary>
    IRepository<Role> Roles { get; }

    /// <summary>
    /// İzin repository'sine erişim sağlar.
    /// </summary>
    IRepository<Permission> Permissions { get; }

    /// <summary>
    /// Refresh Token repository'sine erişim sağlar.
    /// </summary>
    IRepository<RefreshToken> RefreshTokens { get; }

    /// <summary>
    /// Aktivite Log repository'sine erişim sağlar.
    /// </summary>
    IRepository<ActivityLog> ActivityLogs { get; }

    /// <summary>
    /// Profil repository'sine erişim sağlar.
    /// </summary>
    IRepository<Profile> Profiles { get; }

    /// <summary>
    /// Hero repository'sine erişim sağlar.
    /// </summary>
    IRepository<Hero> Heroes { get; }

    /// <summary>
    /// Yetenek repository'sine erişim sağlar.
    /// </summary>
    IRepository<Skill> Skills { get; }

    /// <summary>
    /// Deneyim repository'sine erişim sağlar.
    /// </summary>
    IRepository<Experience> Experiences { get; }

    /// <summary>
    /// Eğitim repository'sine erişim sağlar.
    /// </summary>
    IRepository<Education> Educations { get; }

    /// <summary>
    /// Proje repository'sine erişim sağlar.
    /// </summary>
    IRepository<Project> Projects { get; }

    /// <summary>
    /// Proje Kategori repository'sine erişim sağlar.
    /// </summary>
    IRepository<ProjectCategory> ProjectCategories { get; }

    /// <summary>
    /// Sertifika repository'sine erişim sağlar.
    /// </summary>
    IRepository<Certificate> Certificates { get; }

    /// <summary>
    /// Referans repository'sine erişim sağlar.
    /// </summary>
    IRepository<Testimonial> Testimonials { get; }

    /// <summary>
    /// Blog Yazısı repository'sine erişim sağlar.
    /// </summary>
    IRepository<BlogPost> BlogPosts { get; }

    /// <summary>
    /// Blog Kategori repository'sine erişim sağlar.
    /// </summary>
    IRepository<BlogCategory> BlogCategories { get; }

    /// <summary>
    /// Hizmet repository'sine erişim sağlar.
    /// </summary>
    IRepository<Service> Services { get; }

    /// <summary>
    /// İletişim Mesajı repository'sine erişim sağlar.
    /// </summary>
    IRepository<ContactMessage> ContactMessages { get; }

    /// <summary>
    /// Medya Dosyası repository'sine erişim sağlar.
    /// </summary>
    IRepository<MediaFile> MediaFiles { get; }

    /// <summary>
    /// Etiket repository'sine erişim sağlar.
    /// </summary>
    IRepository<Tag> Tags { get; }

    /// <summary>
    /// Menü Öğesi repository'sine erişim sağlar.
    /// </summary>
    IRepository<MenuItem> MenuItems { get; }

    /// <summary>
    /// Sayfa repository'sine erişim sağlar.
    /// </summary>
    IRepository<Page> Pages { get; }

    /// <summary>
    /// Widget repository'sine erişim sağlar.
    /// </summary>
    IRepository<Widget> Widgets { get; }

    /// <summary>
    /// Tema repository'sine erişim sağlar.
    /// </summary>
    IRepository<Theme> Themes { get; }

    /// <summary>
    /// Site Ayarı repository'sine erişim sağlar.
    /// </summary>
    IRepository<SiteSetting> SiteSettings { get; }

    /// <summary>
    /// Sosyal Medya repository'sine erişim sağlar.
    /// </summary>
    IRepository<SocialMedia> SocialMedias { get; }

    /// <summary>
    /// İletişim Bilgisi repository'sine erişim sağlar.
    /// </summary>
    IRepository<ContactInfo> ContactInfos { get; }

    /// <summary>
    /// Ziyaretçi İstatistiği repository'sine erişim sağlar.
    /// </summary>
    IRepository<VisitorStat> VisitorStats { get; }

    /// <summary>
    /// E-posta Şablonu repository'sine erişim sağlar.
    /// </summary>
    IRepository<EmailTemplate> EmailTemplates { get; }

    /// <summary>
    /// Webhook repository'sine erişim sağlar.
    /// </summary>
    IRepository<Webhook> Webhooks { get; }

    /// <summary>
    /// Feature Flag repository'sine erişim sağlar.
    /// </summary>
    IRepository<FeatureFlag> FeatureFlags { get; }

    /// <summary>
    /// Yedekleme repository'sine erişim sağlar.
    /// </summary>
    IRepository<Backup> Backups { get; }

    /// <summary>
    /// İçerik Revizyon repository'sine erişim sağlar.
    /// </summary>
    IRepository<ContentRevision> ContentRevisions { get; }

    /// <summary>
    /// Tüm değişiklikleri veritabanına kaydeder.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri bir transaction içinde kaydeder.
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Transaction'ı commit eder.
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Transaction'ı rollback eder.
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
