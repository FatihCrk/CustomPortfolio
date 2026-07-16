using Microsoft.AspNetCore.Http;
using Portfolio.Application.Services.Interfaces;
using System.Net;

namespace Portfolio.Api.Middleware;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitMiddleware> _logger;
    private const int DefaultLimit = 100;
    private static readonly TimeSpan DefaultWindow = TimeSpan.FromMinutes(1);

    public RateLimitMiddleware(RequestDelegate next, ILogger<RateLimitMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IRateLimitService rateLimitService)
    {
        // Skip rate limiting for certain paths
        if (ShouldSkipRateLimit(context.Request.Path))
        {
            await _next(context);
            return;
        }

        // Get client identifier (IP address)
        var clientId = GetClientId(context);
        
        // Apply different limits based on endpoint type
        var (limit, window) = GetRateLimitForPath(context.Request.Path);

        var isAllowed = await rateLimitService.IsAllowedAsync(clientId, limit, window);

        if (!isAllowed)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.ContentType = "application/json";
            
            var response = new
            {
                success = false,
                message = "Çok fazla istek. Lütfen biraz bekleyin.",
                retryAfter = window.TotalSeconds
            };

            await context.Response.WriteAsJsonAsync(response);
            _logger.LogWarning("Rate limit exceeded for client: {ClientId}, path: {Path}", clientId, context.Request.Path);
            return;
        }

        await _next(context);
    }

    private string GetClientId(HttpContext context)
    {
        // Check for forwarded IP (behind proxy/load balancer)
        var forwardedIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        
        if (!string.IsNullOrEmpty(forwardedIp))
        {
            return forwardedIp.Split(',').First().Trim();
        }

        // Fall back to remote IP
        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    private (int limit, TimeSpan window) GetRateLimitForPath(string path)
    {
        // More restrictive limits for authentication endpoints
        if (path.StartsWithSegments("/api/auth/login") || 
            path.StartsWithSegments("/api/auth/register") ||
            path.StartsWithSegments("/api/auth/forgot-password"))
        {
            return (5, TimeSpan.FromMinutes(1)); // 5 requests per minute
        }

        // Moderate limits for contact form
        if (path.StartsWithSegments("/api/messages"))
        {
            return (10, TimeSpan.FromHours(1)); // 10 requests per hour
        }

        // Standard limits for API endpoints
        if (path.StartsWithSegments("/api"))
        {
            return (DefaultLimit, DefaultWindow); // 100 requests per minute
        }

        // Relaxed limits for public content
        return (200, TimeSpan.FromMinutes(1)); // 200 requests per minute
    }

    private bool ShouldSkipRateLimit(string path)
    {
        // Skip rate limiting for:
        // - Health checks
        // - Swagger UI
        // - Static files
        
        return path.StartsWithSegments("/health") ||
               path.StartsWithSegments("/swagger") ||
               path.StartsWithSegments("/_framework") ||
               path.StartsWithSegments("/static");
    }
}

public static class RateLimitMiddlewareExtensions
{
    public static IApplicationBuilder UseRateLimit(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RateLimitMiddleware>();
    }
}
