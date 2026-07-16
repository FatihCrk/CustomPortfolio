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
    Task<string> GenerateUniqueSlugAsync<T>(string text, CancellationToken cancellationToken = default) where T : class;
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
/// Sağlık durumu.
/// </summary>
public class HealthStatus
{
    public bool IsHealthy { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
    public Dictionary<string, HealthCheckResult> Checks { get; set; } = new();
}

/// <summary>
/// Sağlık kontrol sonucu.
/// </summary>
public class HealthCheckResult
{
    public bool IsHealthy { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TimeSpan? ResponseTime { get; set; }
    public string? ErrorMessage { get; set; }
}
