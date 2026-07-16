using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Portfolio.Application.DTOs;
using System.Net;
using System.Text.Json;

namespace Portfolio.Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new ApiResponse<string>();
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";

        _logger.LogError(exception, "Unhandled exception occurred");

        switch (exception)
        {
            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                message = "Yetkilendirme hatası. Giriş yapmanız gerekiyor.";
                break;

            case ArgumentException argumentException:
                statusCode = HttpStatusCode.BadRequest;
                message = argumentException.Message;
                break;

            case InvalidOperationException invalidOperationException:
                statusCode = HttpStatusCode.BadRequest;
                message = invalidOperationException.Message;
                break;

            case KeyNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = "Kaynak bulunamadı.";
                break;

            case FluentValidation.ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                message = "Doğrulama hatası.";
                errorResponse.Errors = validationException.Errors.Select(e => e.ErrorMessage).ToList();
                break;

            case TimeoutException:
                statusCode = HttpStatusCode.GatewayTimeout;
                message = "İstek zaman aşımına uğradı. Lütfen tekrar deneyiniz.";
                break;

            case Exception ex when ex.InnerException is FluentValidation.ValidationException innerValidation:
                statusCode = HttpStatusCode.BadRequest;
                message = "Doğrulama hatası.";
                errorResponse.Errors = innerValidation.Errors.Select(e => e.ErrorMessage).ToList();
                break;
        }

        errorResponse.Success = false;
        errorResponse.Message = message;
        
        if (errorResponse.Errors.Count == 0)
        {
            errorResponse.Errors = new List<string> { message };
        }

        response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(errorResponse, options);
        await response.WriteAsync(json);
    }
}

public static class GlobalExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionMiddleware>();
    }
}
