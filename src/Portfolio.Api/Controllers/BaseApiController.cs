using Microsoft.AspNetCore.Mvc;
using Portfolio.Shared.Response;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/v{version:alpha}/[controller]")]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase
{
    /// <summary>
    /// Başarılı işlem yanıtı
    /// </summary>
    protected IActionResult Ok<T>(T data, string? message = null)
    {
        var response = new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message ?? "İşlem başarılı",
            StatusCode = StatusCodes.Status200OK
        };
        return base.Ok(response);
    }

    /// <summary>
    /// Oluşturma işlemi yanıtı (201 Created)
    /// </summary>
    protected IActionResult Created<T>(T data, string routeName, object? routeValues = null, string? message = null)
    {
        var response = new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message ?? "Kayıt başarıyla oluşturuldu",
            StatusCode = StatusCodes.Status201Created
        };
        return CreatedAtRoute(routeName, routeValues, response);
    }

    /// <summary>
    /// İçerik bulunamadı yanıtı
    /// </summary>
    protected IActionResult NotFound(string? message = null)
    {
        var response = new ApiResponse<object>
        {
            Success = false,
            Data = null,
            Message = message ?? "İçerik bulunamadı",
            StatusCode = StatusCodes.Status404NotFound
        };
        return base.NotFound(response);
    }

    /// <summary>
    /// Hatalı istek yanıtı
    /// </summary>
    protected IActionResult BadRequest(string? message = null, IEnumerable<string>? errors = null)
    {
        var response = new ApiResponse<object>
        {
            Success = false,
            Data = null,
            Message = message ?? "Geçersiz istek",
            Errors = errors,
            StatusCode = StatusCodes.Status400BadRequest
        };
        return base.BadRequest(response);
    }

    /// <summary>
    /// Yetkisiz erişim yanıtı
    /// </summary>
    protected IActionResult Unauthorized(string? message = null)
    {
        var response = new ApiResponse<object>
        {
            Success = false,
            Data = null,
            Message = message ?? "Yetkisiz erişim",
            StatusCode = StatusCodes.Status401Unauthorized
        };
        return base.Unauthorized(response);
    }

    /// <summary>
    /// Yasak erişim yanıtı
    /// </summary>
    protected IActionResult Forbidden(string? message = null)
    {
        var response = new ApiResponse<object>
        {
            Success = false,
            Data = null,
            Message = message ?? "Bu kaynağa erişim yasak",
            StatusCode = StatusCodes.Status403Forbidden
        };
        return base.StatusCode(403, response);
    }

    /// <summary>
    /// Sunucu hatası yanıtı
    /// </summary>
    protected IActionResult InternalServerError(string? message = null, string? detail = null)
    {
        var response = new ApiResponse<object>
        {
            Success = false,
            Data = null,
            Message = message ?? "Bir hata oluştu",
            Detail = detail,
            StatusCode = StatusCodes.Status500InternalServerError
        };
        return base.StatusCode(500, response);
    }
}
