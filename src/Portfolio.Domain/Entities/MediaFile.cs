using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class MediaFile : BaseEntity
{
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty; // image, video, document
    public string MimeType { get; set; } = string.Empty;
    public long FileSize { get; set; } // bytes
    public string FilePath { get; set; } = string.Empty;
    public string? Url { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Guid? UploadedBy { get; set; }
    public string? Description { get; set; }
    public string? AltText { get; set; }
    
    // Navigation Properties
    public User? UploadedByUser { get; set; }
}
