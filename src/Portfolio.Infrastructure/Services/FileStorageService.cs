using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Portfolio.Application.Interfaces;
using Portfolio.Shared.Exceptions;

namespace Portfolio.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly string _basePath;
    private readonly ILogger<FileStorageService> _logger;
    private readonly long _maxFileSize = 50 * 1024 * 1024; // 50MB
    private readonly ICollection<string> _allowedImageTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
    private readonly ICollection<string> _allowedDocumentTypes = new[] { "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" };

    public FileStorageService(IConfiguration configuration, ILogger<FileStorageService> logger)
    {
        _basePath = configuration["Storage:LocalPath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        _logger = logger;

        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string? folder = null, CancellationToken cancellationToken = default)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        var safeFileName = $"{Guid.NewGuid()}{ext}";
        var folderPath = string.IsNullOrEmpty(folder) ? _basePath : Path.Combine(_basePath, folder);

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var fullPath = Path.Combine(folderPath, safeFileName);

        await using var fs = new FileStream(fullPath, FileMode.CreateNew);
        await fileStream.CopyToAsync(fs, cancellationToken);

        _logger.LogInformation("Dosya yüklendi: {FilePath}", fullPath);
        return Path.Combine(folder ?? "", safeFileName).Replace("\\", "/");
    }

    public Task DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_basePath, filePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            _logger.LogInformation("Dosya silindi: {FilePath}", fullPath);
        }
        return Task.CompletedTask;
    }

    public string GetFileUrl(string filePath)
    {
        return $"/uploads/{filePath}";
    }

    public Task<string> GenerateThumbnailAsync(string filePath, int width, int height, CancellationToken cancellationToken = default)
    {
        // ImageSharp veya SkiaSharp ile thumbnail oluşturma implementasyonu
        // Şimdilik basit versiyon
        return Task.FromResult(filePath);
    }

    public Task<string> OptimizeImageAsync(string filePath, int? quality = null, CancellationToken cancellationToken = default)
    {
        // WebP dönüşümü ve optimizasyon
        return Task.FromResult(filePath);
    }

    public bool IsValidFileType(string fileName, ICollection<string> allowedTypes)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return allowedTypes.Any(t => t.ToLowerInvariant().EndsWith(ext));
    }

    public bool IsValidFileSize(long fileSize, long maxSizeInBytes)
    {
        return fileSize <= maxSizeInBytes;
    }
}
