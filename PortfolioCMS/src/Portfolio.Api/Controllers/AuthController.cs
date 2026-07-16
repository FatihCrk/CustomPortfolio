using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.DTOs;
using Portfolio.Application.Interfaces;
using Portfolio.Domain.Enums;

namespace Portfolio.Api.Controllers;

public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginRequest request)
    {
        var ipAddress = GetUserIpAddress();
        var userAgent = GetUserAgent();
        
        _logger.LogInformation("Login attempt for user: {Username} from IP: {IpAddress}", 
            request.UsernameOrEmail, ipAddress);

        var result = await _authService.LoginAsync(
            request.UsernameOrEmail, 
            request.Password, 
            ipAddress, 
            userAgent, 
            request.RememberMe);

        if (!result.Success)
        {
            _logger.LogWarning("Login failed for user: {Username}. Reason: {Message}", 
                request.UsernameOrEmail, result.Message);
            return Unauthorized(result);
        }

        // Set refresh token in secure cookie
        Response.Cookies.Append(
            "refreshToken",
            result.Data.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = result.Data.ExpiresAt,
                Path = "/api/auth/refresh-token"
            });

        _logger.LogInformation("Login successful for user: {Username}", request.UsernameOrEmail);
        return Success(result.Data, "Giriş başarılı");
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(Error("Refresh token bulunamadı"));
        }

        var ipAddress = GetUserIpAddress();
        var userAgent = GetUserAgent();

        var result = await _authService.RefreshTokenAsync(refreshToken, ipAddress, userAgent);

        if (!result.Success)
        {
            return Unauthorized(result);
        }

        // Set new refresh token in secure cookie
        Response.Cookies.Append(
            "refreshToken",
            result.Data.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = result.Data.ExpiresAt,
                Path = "/api/auth/refresh-token"
            });

        return Success(result.Data, "Token yenilendi");
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse>> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var userId = GetCurrentUserId();

        if (!string.IsNullOrEmpty(refreshToken))
        {
            await _authService.RevokeTokenAsync(refreshToken, userId);
        }

        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/api/auth/refresh-token"
        });

        _logger.LogInformation("User {UserId} logged out", userId);
        return Success("Çıkış başarılı");
    }

    [HttpPost("logout-all")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse>> LogoutAllDevices()
    {
        var userId = GetCurrentUserId();
        await _authService.RevokeAllTokensAsync(userId);

        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/api/auth/refresh-token"
        });

        _logger.LogInformation("User {UserId} logged out from all devices", userId);
        return Success("Tüm cihazlardan çıkış yapıldı");
    }

    [HttpPost("setup")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> SetupAdmin([FromBody] SetupAdminRequest request)
    {
        var ipAddress = GetUserIpAddress();
        var userAgent = GetUserAgent();

        var result = await _authService.SetupAdminAsync(request, ipAddress, userAgent);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        _logger.LogInformation("Setup completed. Admin user created: {Username}", request.Username);
        return Success(result.Data, "Kurulum başarılı. Admin hesabı oluşturuldu.");
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var result = await _authService.ForgotPasswordAsync(request.Email);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Success("Şifre sıfırlama bağlantısı e-posta adresinize gönderildi.");
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authService.ResetPasswordAsync(request.Token, request.NewPassword);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Success("Şifre başarıyla sıfırlandı.");
    }

    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = GetCurrentUserId();
        var result = await _authService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Success("Şifre başarıyla değiştirildi.");
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserInfo>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<UserInfo>>> GetCurrentUser()
    {
        var userId = GetCurrentUserId();
        var result = await _authService.GetUserByIdAsync(userId);
        
        if (result == null)
        {
            return NotFound(Error("Kullanıcı bulunamadı"));
        }

        return Success(result);
    }
}
