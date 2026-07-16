using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Portfolio.Shared.Exceptions;

namespace Portfolio.Infrastructure.Services;

public interface IStorageService
{
    Task<string> UploadAsync(IFormFile file, string folder, bool overwrite = false);
    Task<bool> DeleteAsync(string filePath);
    Task<byte[]> DownloadAsync(string filePath);
    Task<string> GetUrlAsync(string filePath);
}

public class LocalStorageService : IStorageService
{
    private readonly string _basePath;
    private readonly ILogger<LocalStorageService> _logger;
    private readonly IEnumerable<string> _allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".pdf", ".doc", ".docx", ".zip" };
    private readonly long _maxFileSize = 10 * 1024 * 1024; // 10MB

    public LocalStorageService(IConfiguration configuration, ILogger<LocalStorageService> logger)
    {
        _basePath = configuration["Storage:LocalPath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        _logger = logger;
        
        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);
    }

    public async Task<string> UploadAsync(IFormFile file, string folder, bool overwrite = false)
    {
        if (file.Length == 0 || file.Length > _maxFileSize)
            throw new CustomException($"Dosya boyutu izin verilen limitin üzerinde veya boş. Max: {_maxFileSize / 1024 / 1024}MB");

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(ext))
            throw new CustomException($"Geçersiz dosya tipi. İzin verilenler: {string.Join(", ", _allowedExtensions)}");

        // Güvenli dosya adı oluştur
        var fileName = $"{Guid.NewGuid()}{ext}";
        var folderPath = Path.Combine(_basePath, folder);
        
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var fullPath = Path.Combine(folderPath, fileName);

        await using var stream = file.OpenReadStream();
        await using var fs = new FileStream(fullPath, overwrite ? FileMode.Create : FileMode.CreateNew);
        await stream.CopyToAsync(fs);
        
        _logger.LogInformation("Dosya yüklendi: {FilePath}", fullPath);
        
        return Path.Combine(folder, fileName).Replace("\\", "/");
    }

    public Task<bool> DeleteAsync(string filePath)
    {
        var fullPath = Path.Combine(_basePath, filePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            _logger.LogInformation("Dosya silindi: {FilePath}", fullPath);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<byte[]> DownloadAsync(string filePath)
    {
        var fullPath = Path.Combine(_basePath, filePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
        if (File.Exists(fullPath))
            return Task.FromResult(File.ReadAllBytes(fullPath));
        
        throw new FileNotFoundException("Dosya bulunamadı", filePath);
    }

    public Task<string> GetUrlAsync(string filePath)
    {
        return Task.FromResult($"/uploads/{filePath}");
    }
}
