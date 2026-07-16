using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.DTOs;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class BaseApiController : ControllerBase
{
    protected ApiResponse<T> Success<T>(T data, string message = "İşlem başarılı")
    {
        return ApiResponse<T>.Ok(data, message);
    }

    protected ApiResponse Success(string message = "İşlem başarılı")
    {
        return ApiResponse.Ok(message);
    }

    protected ApiResponse Error(string message, List<string>? errors = null)
    {
        return ApiResponse.Fail(message, errors);
    }

    protected int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    protected string GetUserIpAddress()
    {
        return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }

    protected string GetUserAgent()
    {
        return Request.Headers["User-Agent"].ToString();
    }
}
