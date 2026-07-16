using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Validators;
using Portfolio.Domain.Interfaces;
using Portfolio.Shared.DTOs.Auth;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<LoginRequestDto> _loginValidator;
    private readonly IValidator<SetupAdminRequestDto> _setupValidator;
    private readonly IValidator<ChangePasswordDto> _changePasswordValidator;
    private readonly IValidator<ForgotPasswordDto> _forgotPasswordValidator;
    private readonly IValidator<ResetPasswordDto> _resetPasswordValidator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        IValidator<LoginRequestDto> loginValidator,
        IValidator<SetupAdminRequestDto> setupValidator,
        IValidator<ChangePasswordDto> changePasswordValidator,
        IValidator<ForgotPasswordDto> forgotPasswordValidator,
        IValidator<ResetPasswordDto> resetPasswordValidator,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _loginValidator = loginValidator;
        _setupValidator = setupValidator;
        _changePasswordValidator = changePasswordValidator;
        _forgotPasswordValidator = forgotPasswordValidator;
        _resetPasswordValidator = resetPasswordValidator;
        _logger = logger;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginRequestDto request)
    {
        var validationResult = await _loginValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ApiResponse(false, "Doğrulama hatası", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        var ipAddress = GetClientIpAddress();
        var userAgent = Request.Headers["User-Agent"].FirstOrDefault() ?? "Unknown";

        try
        {
            var result = await _authService.LoginAsync(request, ipAddress, userAgent);
            
            // Set refresh token in secure cookie
            Response.Cookies.Append(
                "refreshToken",
                result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = result.Expiration
                });

            return Ok(new ApiResponse<AuthResponseDto>(true, "Giriş başarılı", result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed for {Email}", request.Email);
            return Unauthorized(new ApiResponse(false, ex.Message));
        }
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        var ipAddress = GetClientIpAddress();
        var userAgent = Request.Headers["User-Agent"].FirstOrDefault() ?? "Unknown";

        try
        {
            var result = await _authService.RefreshTokenAsync(request, ipAddress, userAgent);
            
            Response.Cookies.Append(
                "refreshToken",
                result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = result.Expiration
                });

            return Ok(new ApiResponse<AuthResponseDto>(true, "Token yenilendi", result));
        }
        catch (Exception ex)
        {
            return Unauthorized(new ApiResponse(false, ex.Message));
        }
    }

    [HttpPost("setup")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> SetupAdmin([FromBody] SetupAdminRequestDto request)
    {
        var validationResult = await _setupValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ApiResponse(false, "Doğrulama hatası", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        var ipAddress = GetClientIpAddress();
        var userAgent = Request.Headers["User-Agent"].FirstOrDefault() ?? "Unknown";

        try
        {
            var result = await _authService.RegisterSetupAdminAsync(request, ipAddress, userAgent);
            return Ok(new ApiResponse<AuthResponseDto>(true, "Kurulum başarılı. Super Admin oluşturuldu.", result));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(false, ex.Message));
        }
    }

    [HttpGet("setup/status")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<bool>>> GetSetupStatus()
    {
        var isSetupCompleted = await _authService.IsSetupCompletedAsync();
        return Ok(new ApiResponse<bool>(true, "Kurulum durumu alındı", isSetupCompleted));
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse>> Logout()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var ipAddress = GetClientIpAddress();
        var userAgent = Request.Headers["User-Agent"].FirstOrDefault() ?? "Unknown";

        await _authService.LogoutAsync(userId!, ipAddress, userAgent);
        
        Response.Cookies.Delete("refreshToken");
        
        return Ok(new ApiResponse(true, "Çıkış başarılı"));
    }

    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> ChangePassword([FromBody] ChangePasswordDto request)
    {
        var validationResult = await _changePasswordValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ApiResponse(false, "Doğrulama hatası", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        // TODO: Implement password change logic in AuthService
        
        return Ok(new ApiResponse(true, "Şifre değiştirildi"));
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> ForgotPassword([FromBody] ForgotPasswordDto request)
    {
        var validationResult = await _forgotPasswordValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ApiResponse(false, "Doğrulama hatası", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        // TODO: Implement forgot password logic
        
        // Always return success to prevent email enumeration
        return Ok(new ApiResponse(true, "E-posta gönderildi (eğer kayıtlıysa)"));
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordDto request)
    {
        var validationResult = await _resetPasswordValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ApiResponse(false, "Doğrulama hatası", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        // TODO: Implement reset password logic
        
        return Ok(new ApiResponse(true, "Şifre sıfırlandı"));
    }

    private string GetClientIpAddress()
    {
        var forwardedFor = Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
            return forwardedFor.Split(',').First().Trim();

        var realIp = Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp))
            return realIp;

        return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}

// Generic API Response
public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<string>? Errors { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public ApiResponse(bool success, string message, IEnumerable<string>? errors = null)
    {
        Success = success;
        Message = message;
        Errors = errors;
    }
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }

    public ApiResponse(bool success, string message, T data) : base(success, message)
    {
        Data = data;
    }
}
