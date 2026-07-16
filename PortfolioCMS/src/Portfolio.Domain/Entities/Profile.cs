using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public class Profile : BaseEntity
{
    public int UserId { get; set; }
    public string? Title { get; set; }
    public string? SubTitle { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public string? PhotoUrl { get; set; }
    public string? CvUrl { get; set; }
    public string? Location { get; set; }
    public DateTime? BirthDate { get; set; }
    public bool IsPublished { get; set; } = true;
    
    // Navigation Properties
    public User User { get; set; } = null!;
}
