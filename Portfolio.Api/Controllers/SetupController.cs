using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.DTOs;
using Portfolio.Application.Services.Interfaces;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SetupController : ControllerBase
{
    private readonly ISetupService _setupService;
    private readonly ILogger<SetupController> _logger;

    public SetupController(ISetupService setupService, ILogger<SetupController> logger)
    {
        _setupService = setupService;
        _logger = logger;
    }

    [HttpGet("status")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<bool>>> GetSetupStatus()
    {
        try
        {
            var isCompleted = await _setupService.IsSetupCompletedAsync();
            return Ok(ApiResponse.Ok(isCompleted, isCompleted ? "Kurulum tamamlanmış" : "Kurulum bekliyor"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking setup status");
            return StatusCode(500, ApiResponse.Fail("Kurulum durumu kontrol edilemedi"));
        }
    }

    [HttpPost("complete")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<SetupResultDto>>> CompleteSetup([FromBody] SetupAdminDto dto)
    {
        try
        {
            // Check if setup is already completed
            var isAlreadyCompleted = await _setupService.IsSetupCompletedAsync();
            
            if (isAlreadyCompleted)
            {
                return BadRequest(ApiResponse<SetupResultDto>.Fail("Kurulum zaten tamamlanmış. Bu endpoint artık kullanılamaz."));
            }

            // Validate model
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return BadRequest(ApiResponse<SetupResultDto>.Fail("Geçersiz veri", errors));
            }

            // Complete setup
            var result = await _setupService.CompleteSetupAsync(dto);

            if (!result.Success)
            {
                return BadRequest(ApiResponse<SetupResultDto>.Fail(result.Message));
            }

            _logger.LogInformation("Setup completed successfully by user: {Username}", dto.Username);
            
            return Ok(ApiResponse.Ok(result, "Kurulum başarıyla tamamlandı"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing setup");
            return StatusCode(500, ApiResponse<SetupResultDto>.Fail("Kurulum tamamlanırken bir hata oluştu"));
        }
    }
}
