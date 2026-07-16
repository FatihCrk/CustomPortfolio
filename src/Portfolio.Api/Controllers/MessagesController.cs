using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Interfaces;
using Portfolio.Domain.Entities.Cms;
using Portfolio.Domain.Enums;
using Portfolio.Api.Controllers;
using Portfolio.Shared.Response;

namespace Portfolio.Api.Controllers;

public class MessagesController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService? _emailService;

    public MessagesController(IUnitOfWork unitOfWork, IEmailService? emailService = null)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    /// <summary>
    /// Yeni mesaj oluşturur (public - iletişim formu)
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateMessage([FromBody] MessageCreateDto dto, CancellationToken cancellationToken = default)
    {
        // TODO: Rate limiting kontrolü
        // TODO: Captcha doğrulaması
        
        var message = new ContactMessage
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Subject = dto.Subject,
            Body = dto.Body,
            IpAddress = GetClientIpAddress(),
            UserAgent = Request.Headers.UserAgent.ToString(),
            Status = MessageStatus.Unread,
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.ContactMessage.AddAsync(message, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Admin'e bildirim e-postası (opsiyonel)
        if (_emailService != null && !string.IsNullOrEmpty(dto.Email))
        {
            try
            {
                await _emailService.SendContactReplyEmailAsync(
                    dto.Email, 
                    dto.Subject, 
                    $"Merhaba {dto.Name}, mesajınız alındı. En kısa sürede dönüş yapacağız.",
                    cancellationToken);
            }
            catch (Exception ex)
            {
                // E-posta gönderimi başarısız olsa bile işlem başarılı sayılır
                Console.WriteLine($"E-posta gönderim hatası: {ex.Message}");
            }
        }

        return Created(nameof(GetMessage), new { id = message.Id }, "Mesajınız başarıyla iletildi");
    }

    /// <summary>
    /// Tüm mesajları listeler (admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> GetMessages(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] MessageStatus? status = null,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = "createdDate",
        [FromQuery] bool descending = true,
        CancellationToken cancellationToken = default)
    {
        var messages = await _unitOfWork.ContactMessage.GetAllAsync(
            m => !m.IsDeleted &&
                 (status == null || m.Status == status) &&
                 (string.IsNullOrEmpty(search) || 
                  m.Name.Contains(search) || 
                  m.Email.Contains(search) || 
                  m.Subject.Contains(search)),
            orderBy: q => sortBy.ToLower() switch
            {
                "name" => descending ? q.OrderByDescending(x => x.Name) : q.OrderBy(x => x.Name),
                "email" => descending ? q.OrderByDescending(x => x.Email) : q.OrderBy(x => x.Email),
                "status" => descending ? q.OrderByDescending(x => x.Status) : q.OrderBy(x => x.Status),
                _ => descending ? q.OrderByDescending(x => x.CreatedDate) : q.OrderBy(x => x.CreatedDate)
            },
            pageNumber: page,
            pageSize: pageSize,
            cancellationToken: cancellationToken);

        var totalCount = await _unitOfWork.ContactMessage.CountAsync(
            m => !m.IsDeleted &&
                 (status == null || m.Status == status) &&
                 (string.IsNullOrEmpty(search) ||
                  m.Name.Contains(search) ||
                  m.Email.Contains(search) ||
                  m.Subject.Contains(search)),
            cancellationToken);

        var response = new PagedResponse<ContactMessage>
        {
            Success = true,
            Data = messages.ToList(),
            TotalCount = totalCount,
            PageNumber = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
            Message = "Mesajlar başarıyla getirildi"
        };

        return Ok(response);
    }

    /// <summary>
    /// Mesaj detayını getirir (admin only)
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> GetMessage(Guid id, CancellationToken cancellationToken = default)
    {
        var message = await _unitOfWork.ContactMessage.GetByIdAsync(id, cancellationToken);
        
        if (message == null || message.IsDeleted)
            return NotFound("Mesaj bulunamadı");

        // Mesaj okundu olarak işaretle
        if (message.Status == MessageStatus.Unread)
        {
            message.Status = MessageStatus.Read;
            _unitOfWork.ContactMessage.Update(message);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Ok(new ApiResponse<ContactMessage>
        {
            Success = true,
            Data = message,
            Message = "Mesaj detayı getirildi"
        });
    }

    /// <summary>
    /// Mesaj durumunu günceller (admin only)
    /// </summary>
    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> UpdateMessageStatus(Guid id, [FromBody] MessageStatusUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var message = await _unitOfWork.ContactMessage.GetByIdAsync(id, cancellationToken);
        
        if (message == null || message.IsDeleted)
            return NotFound("Mesaj bulunamadı");

        message.Status = dto.Status;
        _unitOfWork.ContactMessage.Update(message);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Ok(new ApiResponse<ContactMessage>
        {
            Success = true,
            Data = message,
            Message = "Mesaj durumu güncellendi"
        });
    }

    /// <summary>
    /// Mesajı siler (admin only)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> DeleteMessage(Guid id, CancellationToken cancellationToken = default)
    {
        var message = await _unitOfWork.ContactMessage.GetByIdAsync(id, cancellationToken);
        
        if (message == null || message.IsDeleted)
            return NotFound("Mesaj bulunamadı");

        _unitOfWork.ContactMessage.SoftDelete(message);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Mesaj başarıyla silindi"
        });
    }

    /// <summary>
    /// Mesajları CSV olarak dışa aktar (admin only)
    /// </summary>
    [HttpGet("export/csv")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async Task<IActionResult> ExportToCsv(
        [FromQuery] MessageStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var messages = await _unitOfWork.ContactMessage.GetAllAsync(
            m => !m.IsDeleted && (status == null || m.Status == status),
            orderBy: q => q.OrderByDescending(x => x.CreatedDate),
            cancellationToken: cancellationToken);

        var csv = new StringBuilder();
        csv.AppendLine("Id,Tarih,Ad,Eposta,Telefon,Konu,Durum");

        foreach (var msg in messages)
        {
            csv.AppendLine($"{msg.Id},{msg.CreatedDate:yyyy-MM-dd HH:mm:ss},{msg.Name},{msg.Email},{msg.Phone ?? ""},{msg.Subject},{msg.Status}");
        }

        var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
        return File(bytes, "text/csv", $"mesajlar_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv");
    }

    private string? GetClientIpAddress()
    {
        var ip = Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                 Request.Headers["X-Real-IP"].FirstOrDefault() ??
                 HttpContext.Connection.RemoteIpAddress?.ToString();
        return ip;
    }
}

// DTOs
public class MessageCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}

public class MessageStatusUpdateDto
{
    public MessageStatus Status { get; set; }
}
