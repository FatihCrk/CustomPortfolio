using System.Collections.Concurrent;
using Portfolio.Shared.Exceptions;

namespace Portfolio.Api.Middleware;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitMiddleware> _logger;
    
    // In-memory rate limiting (use Redis for distributed scenarios)
    private static readonly ConcurrentDictionary<string, List<DateTime>> _ipRequestHistory = new();
    private static readonly ConcurrentDictionary<string, List<DateTime>> _userRequestHistory = new();
    
    private readonly int _requestsPerMinute;
    private readonly int _requestsPerHour;

    public RateLimitMiddleware(RequestDelegate next, ILogger<RateLimitMiddleware> logger, IConfiguration config)
    {
        _next = next;
        _logger = logger;
        _requestsPerMinute = int.Parse(config["RateLimit:RequestsPerMinute"] ?? "60");
        _requestsPerHour = int.Parse(config["RateLimit:RequestsPerHour"] ?? "1000");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = GetClientIpAddress(context);
        var userId = context.User.Identity?.IsAuthenticated == true 
            ? context.User.FindFirst("sub")?.Value 
            : null;

        // Check IP-based rate limit
        await CheckRateLimitAsync(ip, _ipRequestHistory, "IP", context);
        
        // Check User-based rate limit if authenticated
        if (!string.IsNullOrEmpty(userId))
        {
            await CheckRateLimitAsync(userId, _userRequestHistory, "User", context);
        }

        await _next(context);
    }

    private async Task CheckRateLimitAsync(
        string identifier, 
        ConcurrentDictionary<string, List<DateTime>> history, 
        string type,
        HttpContext context)
    {
        var now = DateTime.UtcNow;
        var oneMinuteAgo = now.AddMinutes(-1);
        var oneHourAgo = now.AddHours(-1);

        var requests = history.GetOrAdd(identifier, _ => new List<DateTime>());

        lock (requests)
        {
            // Clean old entries
            requests.RemoveAll(r => r < oneHourAgo);

            // Count recent requests
            var lastMinuteCount = requests.Count(r => r > oneMinuteAgo);
            var lastHourCount = requests.Count;

            if (lastMinuteCount >= _requestsPerMinute)
            {
                _logger.LogWarning("{Type} rate limit exceeded: {Identifier}", type, identifier);
                throw new RateLimitException($"Çok fazla istek ({type}). Lütfen 1 dakika bekleyin.");
            }

            if (lastHourCount >= _requestsPerHour)
            {
                _logger.LogWarning("{Type} hourly rate limit exceeded: {Identifier}", type, identifier);
                throw new RateLimitException($"Saatlik istek limiti aşıldı ({type}). Lütfen 1 saat bekleyin.");
            }

            // Record this request
            requests.Add(now);
        }

        // Add rate limit headers
        context.Response.Headers["X-RateLimit-Limit-Minute"] = _requestsPerMinute.ToString();
        context.Response.Headers["X-RateLimit-Remaining-Minute"] = (_requestsPerMinute - lastMinuteCount - 1).ToString();
        context.Response.Headers["X-RateLimit-Limit-Hour"] = _requestsPerHour.ToString();
        context.Response.Headers["X-RateLimit-Remaining-Hour"] = (_requestsPerHour - lastHourCount - 1).ToString();
    }

    private string GetClientIpAddress(HttpContext context)
    {
        // Check for forwarded IPs (reverse proxy, load balancer)
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',').First().Trim();
        }

        // Check for real IP header
        var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp))
        {
            return realIp;
        }

        // Fallback to remote IP
        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}

public static class RateLimitMiddlewareExtensions
{
    public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RateLimitMiddleware>();
    }
}
