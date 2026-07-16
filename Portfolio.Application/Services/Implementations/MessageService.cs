using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portfolio.Application.DTOs;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Domain.Entities;
using Portfolio.Persistence;

namespace Portfolio.Application.Services.Implementations;

public class MessageService : IMessageService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<MessageService> _logger;
    private readonly IEmailService? _emailService;
    private readonly string? _adminEmail;

    public MessageService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<MessageService> logger,
        IEmailService? emailService = null)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _emailService = emailService;
        _adminEmail = "admin@portfolio.com"; // TODO: Get from configuration
    }

    public async Task<IEnumerable<MessageDto>> GetAllAsync(string? status = null, string? search = null)
    {
        IQueryable<Message> query = _context.Messages.Where(m => !m.IsDeleted);

        if (!string.IsNullOrEmpty(status))
        {
            query = status.ToLower() switch
            {
                "read" => query.Where(m => m.IsRead),
                "unread" => query.Where(m => !m.IsRead),
                "archived" => query.Where(m => m.IsArchived),
                "replied" => query.Where(m => !string.IsNullOrEmpty(m.Reply)),
                _ => query
            };
        }

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(m => 
                m.Name.Contains(search) || 
                m.Email.Contains(search) || 
                m.Subject.Contains(search) ||
                m.Content.Contains(search));
        }

        var messages = await query
            .OrderByDescending(m => m.CreatedDate)
            .ToListAsync();

        return _mapper.Map<IEnumerable<MessageDto>>(messages);
    }

    public async Task<MessageDto?> GetByIdAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        
        if (message == null || message.IsDeleted)
            return null;

        return _mapper.Map<MessageDto>(message);
    }

    public async Task<MessageDto> CreateAsync(CreateMessageDto dto, string ipAddress, string userAgent)
    {
        var message = _mapper.Map<Message>(dto);
        message.IpAddress = ipAddress;
        message.UserAgent = userAgent;
        message.Status = "New";
        
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Message received from: {Email}", message.Email);

        // Send notification email to admin (if configured)
        try
        {
            if (_emailService != null && !string.IsNullOrEmpty(_adminEmail))
            {
                var messageDto = _mapper.Map<MessageDto>(message);
                await _emailService.SendContactNotificationAsync(_adminEmail, messageDto);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send contact notification email");
        }

        return _mapper.Map<MessageDto>(message);
    }

    public async Task<bool> MarkAsReadAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        
        if (message == null || message.IsDeleted)
            return false;

        message.IsRead = true;
        message.ReadDate = DateTime.UtcNow;
        message.Status = "Read";
        
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Message marked as read: {Id}", id);
        
        return true;
    }

    public async Task<bool> MarkAsUnreadAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        
        if (message == null || message.IsDeleted)
            return false;

        message.IsRead = false;
        message.ReadDate = null;
        message.Status = "New";
        
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> ArchiveAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        
        if (message == null || message.IsDeleted)
            return false;

        message.IsArchived = true;
        message.Status = "Archived";
        
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Message archived: {Id}", id);
        
        return true;
    }

    public async Task<bool> UnarchiveAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        
        if (message == null || message.IsDeleted)
            return false;

        message.IsArchived = false;
        message.Status = message.IsRead ? "Read" : "New";
        
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        
        if (message == null || message.IsDeleted)
            return false;

        message.IsDeleted = true;
        message.DeletedDate = DateTime.UtcNow;
        message.DeletedBy = "system"; // TODO: Get from current user
        
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Message deleted: {Id}", id);
        
        return true;
    }

    public async Task<bool> ReplyAsync(int id, string reply)
    {
        var message = await _context.Messages.FindAsync(id);
        
        if (message == null || message.IsDeleted)
            return false;

        message.Reply = reply;
        message.RepliedDate = DateTime.UtcNow;
        message.Status = "Replied";
        message.IsRead = true;
        message.ReadDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Message replied: {Id}", id);
        
        // TODO: Send reply email to sender
        
        return true;
    }

    public async Task<byte[]> ExportToCsvAsync(IEnumerable<int> ids)
    {
        var messages = await _context.Messages
            .Where(m => ids.Contains(m.Id) && !m.IsDeleted)
            .ToListAsync();

        var csv = new System.Text.StringBuilder();
        csv.AppendLine("ID,Name,Email,Phone,Subject,Content,IpAddress,Status,CreatedDate");

        foreach (var msg in messages)
        {
            var line = $"{msg.Id},\"{EscapeCsv(msg.Name)}\",\"{EscapeCsv(msg.Email)}\",\"{msg.Phone ?? ""}\",\"{EscapeCsv(msg.Subject)}\",\"{EscapeCsv(msg.Content)}\",\"{msg.IpAddress}\",{msg.Status},{msg.CreatedDate:yyyy-MM-dd HH:mm:ss}";
            csv.AppendLine(line);
        }

        return System.Text.Encoding.UTF8.GetBytes(csv.ToString());
    }

    private static string EscapeCsv(string value)
    {
        return value?.Replace("\"", "\"\"") ?? "";
    }
}
