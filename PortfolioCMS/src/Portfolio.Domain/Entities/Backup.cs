using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class Backup : BaseEntity
{
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string BackupType { get; set; } = string.Empty; // Full, Incremental
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
}
