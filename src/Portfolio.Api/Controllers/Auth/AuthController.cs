using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Api.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("setup")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Setup([FromBody] SetupWizardDto model, CancellationToken cancellationToken)
    {
        try
        {
            var (accessToken, refreshToken) = await _authService.RegisterAsync(model, cancellationToken);

            // Set refresh token as HTTP-only cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

            return Ok(new
            {
                AccessToken = accessToken,
                Message = "Setup completed successfully. Welcome!"
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto model, CancellationToken cancellationToken)
    {
        try
        {
            var (accessToken, refreshToken) = await _authService.LoginAsync(
                model.UsernameOrEmail, 
                model.Password, 
                model.RememberMe, 
                cancellationToken);

            // Set refresh token as HTTP-only cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

            return Ok(new
            {
                AccessToken = accessToken,
                ExpiresIn = 3600, // 1 hour
                TokenType = "Bearer"
            });
        }
        catch (SecurityException ex)
        {
            return Unauthorized(new { Message = ex.Message });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            await _authService.LogoutAsync(Guid.Parse(userId), cancellationToken);
        }

        // Remove refresh token cookie
        Response.Cookies.Delete("refreshToken");

        return Ok(new { Message = "Logged out successfully" });
    }

    [HttpPost("logout-all")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LogoutAllDevices(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            await _authService.LogoutAllDevicesAsync(Guid.Parse(userId), cancellationToken);
        }

        Response.Cookies.Delete("refreshToken");

        return Ok(new { Message = "Logged out from all devices successfully" });
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new { Message = "Refresh token not found" });
        }

        try
        {
            var newAccessToken = await _authService.RefreshTokenAsync(refreshToken, cancellationToken);
            
            return Ok(new
            {
                AccessToken = newAccessToken,
                ExpiresIn = 3600,
                TokenType = "Bearer"
            });
        }
        catch (SecurityException ex)
        {
            return Unauthorized(new { Message = ex.Message });
        }
    }

    [HttpGet("setup-status")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSetupStatus(CancellationToken cancellationToken)
    {
        var isCompleted = await _authService.IsSetupCompletedAsync(cancellationToken);
        return Ok(new { IsSetupCompleted = isCompleted });
    }
}
