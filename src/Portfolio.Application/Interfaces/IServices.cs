namespace Portfolio.Application.Interfaces;

/// <summary>
/// E-posta servisi arayüzü.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// E-posta gönderir.
    /// </summary>
    Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);

    /// <summary>
    /// E-posta şablonu ile e-posta gönderir.
    /// </summary>
    Task SendEmailFromTemplateAsync(string to, string templateName, object model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Şifre sıfırlama e-postası gönderir.
    /// </summary>
    Task SendPasswordResetEmailAsync(string to, string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Hoş geldin e-postası gönderir.
    /// </summary>
    Task SendWelcomeEmailAsync(string to, string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// İletişim formu cevabı e-postası gönderir.
    /// </summary>
    Task SendContactReplyEmailAsync(string to, string subject, string message, CancellationToken cancellationToken = default);
}

/// <summary>
/// Dosya depolama servisi arayüzü.
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Dosya yükler.
    /// </summary>
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string? folder = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dosya siler.
    /// </summary>
    Task DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dosya URL'i oluşturur.
    /// </summary>
    string GetFileUrl(string filePath);

    /// <summary>
    /// Thumbnail oluşturur.
    /// </summary>
    Task<string> GenerateThumbnailAsync(string filePath, int width, int height, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resmi optimize eder (WebP dönüşümü).
    /// </summary>
    Task<string> OptimizeImageAsync(string filePath, int? quality = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dosya türünü kontrol eder.
    /// </summary>
    bool IsValidFileType(string fileName, ICollection<string> allowedTypes);

    /// <summary>
    /// Dosya boyutunu kontrol eder.
    /// </summary>
    bool IsValidFileSize(long fileSize, long maxSizeInBytes);
}

/// <summary>
/// Cache servisi arayüzü.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Cache'den veri alır.
    /// </summary>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cache'e veri ekler.
    /// </summary>
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cache'den veri siler.
    /// </summary>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Pattern ile cache siler.
    /// </summary>
    Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cache'i temizler.
    /// </summary>
    Task ClearAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Anahtarın cache'de olup olmadığını kontrol eder.
    /// </summary>
    Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Prefix ile cache siler.
    /// </summary>
    Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default);
}

/// <summary>
/// Log servisi arayüzü.
/// </summary>
public interface ILogService
{
    /// <summary>
    /// Bilgi logu yazar.
    /// </summary>
    void Information(string message, params object[] args);

    /// <summary>
    /// Uyarı logu yazar.
    /// </summary>
    void Warning(string message, params object[] args);

    /// <summary>
    /// Hata logu yazar.
    /// </summary>
    void Error(string message, Exception? ex = null, params object[] args);

    /// <summary>
    /// Kritik hata logu yazar.
    /// </summary>
    void Critical(string message, Exception? ex = null, params object[] args);

    /// <summary>
    /// Debug logu yazar.
    /// </summary>
    void Debug(string message, params object[] args);

    /// <summary>
    /// Aktivite logu yazar.
    /// </summary>
    Task LogActivityAsync(
        string activityType,
        string description,
        Guid? userId = null,
        string? entityName = null,
        Guid? entityId = null,
        string? oldValues = null,
        string? newValues = null,
        bool isSuccess = true,
        string? errorMessage = null,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Rate Limiting servisi arayüzü.
/// </summary>
public interface IRateLimitService
{
    /// <summary>
    /// Rate limit kontrolü yapar.
    /// </summary>
    Task<bool> IsAllowedAsync(string identifier, int limit, TimeSpan window, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kalan istek sayısını döner.
    /// </summary>
    Task<int> GetRemainingRequestsAsync(string identifier, int limit, TimeSpan window, CancellationToken cancellationToken = default);

    /// <summary>
    /// Rate limit bilgilerini sıfırlar.
    /// </summary>
    Task ResetAsync(string identifier, CancellationToken cancellationToken = default);
}

/// <summary>
/// Slug servisi arayüzü.
/// </summary>
public interface ISlugService
{
    /// <summary>
    /// Metinden slug oluşturur.
    /// </summary>
    string GenerateSlug(string text);

    /// <summary>
    /// Slug'ın benzersiz olup olmadığını kontrol eder.
    /// </summary>
    Task<string> GenerateUniqueSlugAsync<T>(string text, int? excludeId = null, CancellationToken cancellationToken = default) where T : class;
    
    /// <summary>
    /// Slug'ın benzersiz olup olmadığını kontrol eder.
    /// </summary>
    Task<bool> IsSlugUniqueAsync<T>(string slug, int? excludeId = null, CancellationToken cancellationToken = default) where T : class;
}

/// <summary>
/// Health Check servisi arayüzü.
/// </summary>
public interface IHealthCheckService
{
    /// <summary>
    /// Sistem sağlığını kontrol eder.
    /// </summary>
    Task<HealthStatus> CheckHealthAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Veritabanı sağlığını kontrol eder.
    /// </summary>
    Task<bool> CheckDatabaseHealthAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Cache sağlığını kontrol eder.
    /// </summary>
    Task<bool> CheckCacheHealthAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Dosya depolama sağlığını kontrol eder.
    /// </summary>
    Task<bool> CheckStorageHealthAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Proje servisi arayüzü.
/// </summary>
public interface IProjectService
{
    /// <summary>
    /// ID ile proje getirir.
    /// </summary>
    Task<Project?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tüm projeleri getirir.
    /// </summary>
    Task<IEnumerable<Project>> GetAllAsync(bool includeInactive = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sayfalı proje listesi getirir.
    /// </summary>
    Task<(IEnumerable<Project> Items, int TotalCount)> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? searchTerm = null,
        int? categoryId = null,
        bool? isFeatured = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni proje oluşturur.
    /// </summary>
    Task<Project> CreateAsync(Project project, CancellationToken cancellationToken = default);

    /// <summary>
    /// Projeyi günceller.
    /// </summary>
    Task<Project> UpdateAsync(Project project, CancellationToken cancellationToken = default);

    /// <summary>
    /// Projeyi siler.
    /// </summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kategoriye göre projeleri getirir.
    /// </summary>
    Task<IEnumerable<Project>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Öne çıkan projeleri getirir.
    /// </summary>
    Task<IEnumerable<Project>> GetFeaturedAsync(int count = 6, CancellationToken cancellationToken = default);

    /// <summary>
    /// Slug'ın benzersiz olup olmadığını kontrol eder.
    /// </summary>
    Task<bool> ExistsBySlugAsync(string slug, int? excludeId = null, CancellationToken cancellationToken = default);
}

/// <summary>
/// Mesaj servisi arayüzü.
/// </summary>
public interface IMessageService
{
    /// <summary>
    /// Yeni mesaj oluşturur.
    /// </summary>
    Task<Message> CreateAsync(Message message, CancellationToken cancellationToken = default);

    /// <summary>
    /// ID ile mesaj getirir.
    /// </summary>
    Task<Message?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sayfalı mesaj listesi getirir.
    /// </summary>
    Task<(IEnumerable<Message> Items, int TotalCount)> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        string? searchTerm = null,
        MessageStatus? status = null,
        bool? isRead = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Mesajı okundu olarak işaretler.
    /// </summary>
    Task MarkAsReadAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Mesajı okunmadı olarak işaretler.
    /// </summary>
    Task MarkAsUnreadAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Mesajı arşivler.
    /// </summary>
    Task ArchiveAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Mesajı siler.
    /// </summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Okunmamış mesaj sayısını döner.
    /// </summary>
    Task<int> GetUnreadCountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Son mesajları getirir.
    /// </summary>
    Task<IEnumerable<Message>> GetRecentAsync(int count = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Mesajları CSV olarak dışa aktarır.
    /// </summary>
    Task ExportToCsvAsync(IEnumerable<Message> messages, Stream output, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yöneticiye bildirim e-postası gönderir.
    /// </summary>
    Task SendNotificationAsync(Message message, string adminEmail, CancellationToken cancellationToken = default);
}
