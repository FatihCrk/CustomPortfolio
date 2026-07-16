using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Portfolio.Application.DTOs;
using Portfolio.Application.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Portfolio.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<FileStorageService> _logger;
    private readonly string _uploadsFolder;

    // Allowed file types
    private static readonly Dictionary<string, string[]> AllowedContentTypes = new()
    {
        { "image", new[] { "image/jpeg", "image/png", "image/gif", "image/webp", "image/svg+xml" } },
        { "document", new[] { "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" } },
        { "video", new[] { "video/mp4", "video/webm", "video/ogg" } },
        { "audio", new[] { "audio/mpeg", "audio/wav", "audio/ogg" } }
    };

    // Max file sizes (in bytes)
    private const long MaxImageSize = 5 * 1024 * 1024; // 5MB
    private const long MaxDocumentSize = 10 * 1024 * 1024; // 10MB
    private const long MaxVideoSize = 50 * 1024 * 1024; // 50MB

    public FileStorageService(IWebHostEnvironment environment, ILogger<FileStorageService> logger)
    {
        _environment = environment;
        _logger = logger;
        _uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
        
        if (!Directory.Exists(_uploadsFolder))
        {
            Directory.CreateDirectory(_uploadsFolder);
        }
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string folder = "uploads")
    {
        try
        {
            // Validate content type
            if (!IsValidContentType(contentType))
            {
                throw new InvalidOperationException($"Invalid file type: {contentType}");
            }

            // Validate file size
            var maxSize = GetMaxFileSize(contentType);
            if (fileStream.Length > maxSize)
            {
                throw new InvalidOperationException($"File size exceeds maximum allowed size of {maxSize / 1024 / 1024}MB");
            }

            // Sanitize filename
            var sanitizedFileName = SanitizeFileName(fileName);
            
            // Generate unique filename to prevent overwrites
            var uniqueFileName = $"{Guid.NewGuid()}_{sanitizedFileName}";
            
            // Create folder path
            var folderPath = Path.Combine(_uploadsFolder, folder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Full file path
            var filePath = Path.Combine(folderPath, uniqueFileName);

            // Save file
            using (var outputStream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(outputStream);
            }

            // Generate thumbnail for images
            string? thumbnailUrl = null;
            if (contentType.StartsWith("image"))
            {
                thumbnailUrl = await GenerateThumbnailAsync(filePath, uniqueFileName, folder);
            }

            _logger.LogInformation("File uploaded successfully: {FilePath}", filePath);
            
            return $"/uploads/{folder}/{uniqueFileName}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file: {FileName}", fileName);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            // Convert URL path to physical path
            var physicalPath = GetPhysicalPath(filePath);

            if (!File.Exists(physicalPath))
            {
                _logger.LogWarning("File not found for deletion: {FilePath}", filePath);
                return false;
            }

            File.Delete(physicalPath);
            _logger.LogInformation("File deleted successfully: {FilePath}", filePath);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file: {FilePath}", filePath);
            return false;
        }
    }

    public async Task<byte[]> DownloadAsync(string filePath)
    {
        var physicalPath = GetPhysicalPath(filePath);

        if (!File.Exists(physicalPath))
        {
            throw new FileNotFoundException("File not found", filePath);
        }

        return await File.ReadAllBytesAsync(physicalPath);
    }

    public Task<bool> ExistsAsync(string filePath)
    {
        var physicalPath = GetPhysicalPath(filePath);
        return Task.FromResult(File.Exists(physicalPath));
    }

    public Task<string> GetUrlAsync(string filePath)
    {
        // Already stored as URL path
        return Task.FromResult(filePath);
    }

    private bool IsValidContentType(string contentType)
    {
        if (string.IsNullOrEmpty(contentType))
            return false;

        var mainType = contentType.Split('/')[0].ToLower();
        return AllowedContentTypes.ContainsKey(mainType) && 
               AllowedContentTypes[mainType].Contains(contentType.ToLower());
    }

    private long GetMaxFileSize(string contentType)
    {
        var mainType = contentType.Split('/')[0].ToLower();
        return mainType switch
        {
            "image" => MaxImageSize,
            "document" => MaxDocumentSize,
            "video" => MaxVideoSize,
            "audio" => MaxVideoSize,
            _ => MaxImageSize
        };
    }

    private string SanitizeFileName(string fileName)
    {
        // Remove special characters and spaces
        var sanitized = Encoding.ASCII.GetString(Encoding.GetEncoding("ISO-8859-1")
            .GetBytes(fileName));
        
        sanitized = new string(sanitized.Where(c => char.IsLetterOrDigit(c) || c == '.' || c == '-' || c == '_').ToArray());
        
        // Ensure unique filename
        if (string.IsNullOrEmpty(sanitized))
        {
            sanitized = $"file_{Guid.NewGuid()}";
        }

        return sanitized;
    }

    private string GetPhysicalPath(string filePath)
    {
        // Convert /uploads/folder/file.jpg to physical path
        var relativePath = filePath.TrimStart('/');
        return Path.Combine(_environment.WebRootPath, relativePath);
    }

    private async Task<string?> GenerateThumbnailAsync(string filePath, string fileName, string folder)
    {
        try
        {
            // For now, just return the original image as thumbnail
            // In production, you would use ImageSharp or similar to create actual thumbnails
            
            var thumbnailFolder = Path.Combine(_uploadsFolder, folder, "thumbnails");
            if (!Directory.Exists(thumbnailFolder))
            {
                Directory.CreateDirectory(thumbnailFolder);
            }

            var thumbnailPath = Path.Combine(thumbnailFolder, $"thumb_{fileName}");
            
            // Copy original as thumbnail (placeholder - in production, resize)
            File.Copy(filePath, thumbnailPath, true);
            
            return $"/uploads/{folder}/thumbnails/thumb_{fileName}";
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not generate thumbnail for: {FileName}", fileName);
            return null;
        }
    }
}
