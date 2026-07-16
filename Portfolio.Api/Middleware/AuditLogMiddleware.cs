using Microsoft.AspNetCore.Http;
using Portfolio.Application.Services.Interfaces;
using System.Text.Json;

namespace Portfolio.Api.Middleware;

public class AuditLogMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuditLogMiddleware> _logger;

    public AuditLogMiddleware(RequestDelegate next, ILogger<AuditLogMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IAuditLogService auditLogService)
    {
        // Only log API requests
        if (!context.Request.Path.StartsWithSegments("/api"))
        {
            await _next(context);
            return;
        }

        // Capture request details
        var userId = context.User.FindFirst("sub")?.Value ?? "anonymous";
        var userName = context.User.FindFirst("username")?.Value ?? "anonymous";
        var ipAddress = GetIpAddress(context);
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        var method = context.Request.Method;
        var path = context.Request.Path.ToString();
        var queryString = context.Request.QueryString.ToString();

        // Log the request
        try
        {
            var entityType = ExtractEntityType(path);
            var action = MapHttpMethodToAction(method);

            if (!string.IsNullOrEmpty(entityType) && !string.IsNullOrEmpty(action))
            {
                // Create audit log entry (async fire-and-forget to not block request)
                _ = Task.Run(async () =>
                {
                    try
                    {
                        // In a real implementation, you would create an AuditLog entity and save it
                        _logger.LogInformation(
                            "Audit: {Action} {EntityType} by User:{UserId} ({UserName}) from IP:{IpAddress}",
                            action, entityType, userId, userName, ipAddress);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to create audit log entry");
                    }
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in audit logging");
        }

        await _next(context);
    }

    private string GetIpAddress(HttpContext context)
    {
        var forwardedIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        
        if (!string.IsNullOrEmpty(forwardedIp))
        {
            return forwardedIp.Split(',').First().Trim();
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    private string? ExtractEntityType(string path)
    {
        // Extract entity type from API path
        // e.g., /api/projects -> Projects
        var segments = path.Trim('/').Split('/');
        
        if (segments.Length >= 2 && segments[0] == "api")
        {
            return segments[1];
        }

        return null;
    }

    private string MapHttpMethodToAction(string method)
    {
        return method switch
        {
            "GET" => "Read",
            "POST" => "Create",
            "PUT" => "Update",
            "PATCH" => "Update",
            "DELETE" => "Delete",
            _ => method
        };
    }
}

public static class AuditLogMiddlewareExtensions
{
    public static IApplicationBuilder UseAuditLog(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuditLogMiddleware>();
    }
}
