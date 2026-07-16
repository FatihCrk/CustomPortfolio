using System.Net;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Shared.Exceptions;

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

        var errorResponse = new ErrorResponse();
        
        switch (exception)
        {
            case ValidationException validationEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = "Doğrulama hatası",
                    Errors = validationEx.Errors
                };
                _logger.LogWarning(validationEx, "Validation error occurred");
                break;

            case UnauthorizedException unauthorizedEx:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = unauthorizedEx.Message
                };
                _logger.LogWarning(unauthorizedEx, "Unauthorized access attempt");
                break;

            case ForbiddenException forbiddenEx:
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = forbiddenEx.Message
                };
                _logger.LogWarning(forbiddenEx, "Forbidden access attempt");
                break;

            case NotFoundException notFoundEx:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = notFoundEx.Message
                };
                _logger.LogWarning(notFoundEx, "Resource not found");
                break;

            case BusinessException businessEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = businessEx.Message
                };
                _logger.LogWarning(businessEx, "Business rule violation");
                break;

            case InvalidOperationException invalidOpEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = invalidOpEx.Message
                };
                _logger.LogWarning(invalidOpEx, "Invalid operation");
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz."
                };
                _logger.LogError(exception, "Unhandled exception occurred");
                break;
        }

        // Don't expose stack trace in production
        if (context.RequestServices.GetService<IHostEnvironment>()?.IsDevelopment() == true)
        {
            errorResponse.StackTrace = exception.StackTrace;
        }

        await response.WriteAsJsonAsync(errorResponse);
    }
}

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string>? Errors { get; set; }
    public string? StackTrace { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

// Extension method for easy registration
public static class GlobalExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionMiddleware>();
    }
}
