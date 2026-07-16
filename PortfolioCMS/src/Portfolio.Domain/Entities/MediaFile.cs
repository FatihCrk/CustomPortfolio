using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class MediaFile : BaseEntity
{
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty; // image, video, document, etc.
    public string MimeType { get; set; } = string.Empty;
    public long FileSize { get; set; } // in bytes
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? AltText { get; set; }
    public int UploadedByUserId { get; set; }
    public bool IsPublic { get; set; } = true;
    
    // Navigation Properties
    public User? UploadedBy { get; set; }
}
