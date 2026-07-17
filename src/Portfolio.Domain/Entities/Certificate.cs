using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public class Certificate : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IssuingOrganization { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? CredentialUrl { get; set; }
    public string? CredentialId { get; set; }
    public Guid? MediaFileId { get; set; }
    public int SortOrder { get; set; } = 0;
    
    // Navigation Properties
    public MediaFile? MediaFile { get; set; }
}
