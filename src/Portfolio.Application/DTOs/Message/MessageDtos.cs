using Portfolio.Domain.Enums;

namespace Portfolio.Application.DTOs.Message;

/// <summary>
/// Mesaj DTO'si.
/// </summary>
public class MessageDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public MessageStatus Status { get; set; }
    public bool IsRead => Status == MessageStatus.Read || Status == MessageStatus.Replied;
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? DeviceInfo { get; set; }
    public string? BrowserInfo { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime? RepliedAt { get; set; }
    public string? Reply { get; set; }
    public bool IsSpam { get; set; }
}

/// <summary>
/// Mesaj oluşturma DTO'si (İletişim formu için).
/// </summary>
public class CreateMessageDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    
    // Captcha doğrulama için
    public string? CaptchaToken { get; set; }
}

/// <summary>
/// Mesaj cevap DTO'si.
/// </summary>
public class ReplyMessageDto
{
    public string Reply { get; set; } = string.Empty;
    public bool SendEmailNotification { get; set; } = true;
}

/// <summary>
/// Sayfalı mesaj listesi response DTO'si.
/// </summary>
public class PagedMessagesResponseDto
{
    public List<MessageDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;
    public int UnreadCount { get; set; }
}

/// <summary>
/// Mesaj filtreleme parametreleri.
/// </summary>
public class MessageFilterDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SearchTerm { get; set; }
    public MessageStatus? Status { get; set; }
    public bool? IsRead { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? SortBy { get; set; } = "CreatedDate";
    public bool IsDescending { get; set; } = true;
}

/// <summary>
/// Mesaj istatistikleri DTO'si.
/// </summary>
public class MessageStatsDto
{
    public int TotalMessages { get; set; }
    public int UnreadCount { get; set; }
    public int ReadCount { get; set; }
    public int ArchivedCount { get; set; }
    public int SpamCount { get; set; }
    public int RepliedCount { get; set; }
    public int TodayCount { get; set; }
    public int ThisWeekCount { get; set; }
    public int ThisMonthCount { get; set; }
}
