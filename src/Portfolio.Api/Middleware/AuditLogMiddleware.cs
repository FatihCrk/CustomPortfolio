using System.Text.Json;
using Portfolio.Domain.Entities.Audit;
using Portfolio.Domain.Interfaces;

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

    public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
    {
        var startTime = DateTime.UtcNow;
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Capture request details
        var request = context.Request;
        var requestBody = await ReadRequestBodyAsync(request);
        
        // Process request
        await _next(context);

        stopwatch.Stop();
        var duration = stopwatch.ElapsedMilliseconds;

        // Capture response details
        var response = context.Response;
        
        // Only log significant requests (not static files, health checks, etc.)
        if (ShouldLogRequest(request.Path))
        {
            var userId = context.User.Identity?.IsAuthenticated == true 
                ? context.User.FindFirst("sub")?.Value 
                : null;

            var auditLog = new AuditLog
            {
                UserId = userId,
                Action = request.Method,
                Endpoint = request.Path.ToString(),
                QueryString = request.QueryString.ToString(),
                RequestBody = requestBody,
                ResponseStatusCode = response.StatusCode,
                IpAddress = GetClientIpAddress(context),
                UserAgent = request.Headers["User-Agent"].FirstOrDefault(),
                Browser = ParseBrowser(request.Headers["User-Agent"].FirstOrDefault()),
                Device = ParseDevice(request.Headers["User-Agent"].FirstOrDefault()),
                DurationMs = duration,
                CreatedDate = startTime
            };

            // Add to audit log repository (fire and forget, don't block response)
            try
            {
                await unitOfWork.Repository<AuditLog>().AddAsync(auditLog);
                await unitOfWork.SaveChangesAsync(default);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save audit log");
            }
        }
    }

    private async Task<string?> ReadRequestBodyAsync(HttpRequest request)
    {
        try
        {
            if (!request.Body.CanRead || request.ContentLength == 0 || request.ContentLength > 10000)
                return null;

            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            
            // Truncate if too long
            return body.Length > 2000 ? body.Substring(0, 2000) + "... (truncated)" : body;
        }
        catch
        {
            return null;
        }
    }

    private bool ShouldLogRequest(PathString path)
    {
        var pathValue = path.ToString().ToLower();
        
        // Skip static files, health checks, swagger
        if (pathValue.StartsWith("/health") ||
            pathValue.StartsWith("/swagger") ||
            pathValue.StartsWith("/api-docs") ||
            pathValue.EndsWith(".js") ||
            pathValue.EndsWith(".css") ||
            pathValue.EndsWith(".png") ||
            pathValue.EndsWith(".jpg") ||
            pathValue.EndsWith(".ico"))
        {
            return false;
        }

        return true;
    }

    private string GetClientIpAddress(HttpContext context)
    {
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
            return forwardedFor.Split(',').First().Trim();

        var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp))
            return realIp;

        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    private string ParseBrowser(string? userAgent)
    {
        if (string.IsNullOrEmpty(userAgent)) return "Unknown";
        
        if (userAgent.Contains("Edg")) return "Edge";
        if (userAgent.Contains("Chrome")) return "Chrome";
        if (userAgent.Contains("Firefox")) return "Firefox";
        if (userAgent.Contains("Safari")) return "Safari";
        if (userAgent.Contains("MSIE") || userAgent.Contains("Trident")) return "Internet Explorer";
        
        return "Other";
    }

    private string ParseDevice(string? userAgent)
    {
        if (string.IsNullOrEmpty(userAgent)) return "Unknown";
        
        if (userAgent.Contains("Mobile")) return "Mobile";
        if (userAgent.Contains("Tablet")) return "Tablet";
        if (userAgent.Contains("iPad")) return "Tablet";
        
        return "Desktop";
    }
}

public static class AuditLogMiddlewareExtensions
{
    public static IApplicationBuilder UseAuditLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuditLogMiddleware>();
    }
}
