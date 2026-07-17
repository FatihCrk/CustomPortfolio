using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;

namespace Portfolio.Api.Middlewares;

public class AuditLogMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuditLogMiddleware> _logger;

    public AuditLogMiddleware(RequestDelegate next, ILogger<AuditLogMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = System.DateTime.UtcNow;
        var method = context.Request.Method;
        var path = context.Request.Path;
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var userAgent = context.Request.Headers["User-Agent"].ToString();

        // Only log API requests
        if (path.StartsWithSegments("/api"))
        {
            try
            {
                await _next(context);
                
                var duration = System.DateTime.UtcNow - startTime;
                var statusCode = context.Response.StatusCode;

                _logger.LogInformation(
                    "Audit: {Method} {Path} | IP: {IP} | Status: {StatusCode} | Duration: {Duration}ms | UserAgent: {UserAgent}",
                    method, path, ip, statusCode, duration.TotalMilliseconds, userAgent);
            }
            catch
            {
                throw;
            }
        }
        else
        {
            await _next(context);
        }
    }
}
