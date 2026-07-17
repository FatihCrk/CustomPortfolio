using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Portfolio.Api.Middlewares;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly ConcurrentDictionary<string, RateLimitInfo> _rateLimitStore = new();
    private const int MaxRequests = 100;
    private static readonly TimeSpan Window = TimeSpan.FromMinutes(1);

    public RateLimitMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var now = DateTime.UtcNow;

        if (!_rateLimitStore.TryGetValue(ip, out var rateLimitInfo))
        {
            rateLimitInfo = new RateLimitInfo();
            _rateLimitStore[ip] = rateLimitInfo;
        }

        // Clean old entries
        if (now - rateLimitInfo.WindowStart > Window)
        {
            rateLimitInfo.RequestCount = 0;
            rateLimitInfo.WindowStart = now;
        }

        rateLimitInfo.RequestCount++;

        if (rateLimitInfo.RequestCount > MaxRequests)
        {
            context.Response.StatusCode = 429; // Too Many Requests
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"success\":false,\"message\":\"Too many requests. Please try again later.\"}");
            return;
        }

        context.Response.Headers.Add("X-RateLimit-Limit", MaxRequests.ToString());
        context.Response.Headers.Add("X-RateLimit-Remaining", (MaxRequests - rateLimitInfo.RequestCount).ToString());

        await _next(context);
    }

    private class RateLimitInfo
    {
        public int RequestCount { get; set; }
        public DateTime WindowStart { get; set; } = DateTime.UtcNow;
    }
}
