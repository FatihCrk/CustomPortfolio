using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.DTOs;
using Portfolio.Application.Interfaces;
using Portfolio.Domain.Enums;

namespace Portfolio.Api.Controllers;

public class MessagesController : BaseApiController
{
    private readonly IMessageService _messageService;
    private readonly ILogger<MessagesController> _logger;

    public MessagesController(IMessageService messageService, ILogger<MessagesController> logger)
    {
        _messageService = messageService;
        _logger = logger;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> CreateMessage([FromBody] CreateContactMessageDto dto)
    {
        var ipAddress = GetUserIpAddress();
        var userAgent = GetUserAgent();

        var result = await _messageService.CreateMessageAsync(dto, ipAddress, userAgent);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        _logger.LogInformation("New contact message received from: {Email}", dto.Email);
        return CreatedAtAction(nameof(GetMessages), new { }, Success("Mesajınız alındı. Teşekkürler!"));
    }

    [HttpGet]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ContactMessageDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ContactMessageDto>>>> GetMessages(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? status = null,
        [FromQuery] string sortBy = "CreatedDate",
        [FromQuery] bool sortDescending = true)
    {
        var result = await _messageService.GetMessagesAsync(
            pageNumber, pageSize, searchTerm, status, sortBy, sortDescending);

        return Success(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    [ProducesResponseType(typeof(ApiResponse<ContactMessageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ContactMessageDto>>> GetMessage(int id)
    {
        var userId = GetCurrentUserId();
        var result = await _messageService.GetMessageByIdAsync(id, userId);

        if (result == null)
        {
            return NotFound(Error("Mesaj bulunamadı"));
        }

        return Success(result);
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    [ProducesResponseType(typeof(ApiResponse<ContactMessageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ContactMessageDto>>> UpdateMessageStatus(
        int id, 
        [FromBody] UpdateMessageStatusDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(Error("ID eşleşmiyor"));
        }

        var userId = GetCurrentUserId();
        var result = await _messageService.UpdateMessageStatusAsync(dto, userId);

        if (result == null)
        {
            return NotFound(Error("Mesaj bulunamadı"));
        }

        return Success(result, "Mesaj durumu güncellendi");
    }

    [HttpPut("{id}/read")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> MarkAsRead(int id)
    {
        var userId = GetCurrentUserId();
        var result = await _messageService.MarkAsReadAsync(id, userId);

        if (!result)
        {
            return NotFound(Error("Mesaj bulunamadı"));
        }

        return Success("Mesaj okundu olarak işaretlendi");
    }

    [HttpPut("{id}/archive")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> ArchiveMessage(int id)
    {
        var userId = GetCurrentUserId();
        var result = await _messageService.ArchiveMessageAsync(id, userId);

        if (!result)
        {
            return NotFound(Error("Mesaj bulunamadı"));
        }

        return Success("Mesaj arşivlendi");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> DeleteMessage(int id)
    {
        var userId = GetCurrentUserId();
        var result = await _messageService.DeleteMessageAsync(id, userId);

        if (!result)
        {
            return NotFound(Error("Mesaj bulunamadı"));
        }

        _logger.LogInformation("Message deleted: ID {Id} by User {UserId}", id, userId);
        return Success("Mesaj silindi");
    }

    [HttpGet("export/csv")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    [Produces("text/csv")]
    public async Task<IActionResult> ExportToCsv(
        [FromQuery] string? status = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var csvContent = await _messageService.ExportToCsvAsync(status, startDate, endDate);
        
        var fileName = $"messages_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
        return File(System.Text.Encoding.UTF8.GetBytes(csvContent), "text/csv", fileName);
    }

    [HttpGet("stats")]
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<object>>> GetStats()
    {
        var stats = await _messageService.GetStatsAsync();
        return Success(stats, "İstatistikler getirildi");
    }
}
