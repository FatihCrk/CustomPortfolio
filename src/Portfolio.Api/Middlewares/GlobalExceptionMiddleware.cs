using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Portfolio.Api.Middlewares;

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

        var errorResponse = new
        {
            success = false,
            message = "An error occurred while processing your request.",
            error = GetErrorMessage(exception),
            statusCode = (int)HttpStatusCode.InternalServerError,
            timestamp = DateTime.UtcNow
        };

        // Log the exception
        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        switch (exception)
        {
            case ArgumentException argEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = errorResponse with 
                { 
                    message = argEx.Message, 
                    statusCode = (int)HttpStatusCode.BadRequest 
                };
                break;

            case UnauthorizedAccessException:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse = errorResponse with 
                { 
                    message = "Unauthorized access.", 
                    statusCode = (int)HttpStatusCode.Unauthorized 
                };
                break;

            case KeyNotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse = errorResponse with 
                { 
                    message = "Resource not found.", 
                    statusCode = (int)HttpStatusCode.NotFound 
                };
                break;
                
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(errorResponse, options);
        await response.WriteAsync(json);
    }

    private string GetErrorMessage(Exception ex)
    {
#if DEBUG
        return ex.ToString();
#else
        return "Internal server error occurred.";
#endif
    }
}
