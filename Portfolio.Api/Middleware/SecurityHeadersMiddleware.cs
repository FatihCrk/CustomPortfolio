using Microsoft.AspNetCore.Http;
using System.Net;

namespace Portfolio.Api.Middleware;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var response = context.Response;

        // Prevent clickjacking attacks
        response.Headers.Append("X-Frame-Options", "DENY");

        // Enable XSS filter in browsers
        response.Headers.Append("X-Content-Type-Options", "nosniff");
        response.Headers.Append("X-XSS-Protection", "1; mode=block");

        // Strict Transport Security (HSTS)
        response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");

        // Referrer Policy
        response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");

        // Content Security Policy
        var csp = "default-src 'self'; " +
                  "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdn.jsdelivr.net; " +
                  "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdn.jsdelivr.net; " +
                  "font-src 'self' https://fonts.gstatic.com; " +
                  "img-src 'self' data: https: blob:; " +
                  "connect-src 'self'; " +
                  "frame-ancestors 'none'; " +
                  "base-uri 'self'; " +
                  "form-action 'self'";

        response.Headers.Append("Content-Security-Policy", csp);

        // Permissions Policy
        var permissions = "geolocation=(), microphone=(), camera=(), payment=(), usb=(), magnetometer=(), gyroscope=(), accelerometer=()";
        response.Headers.Append("Permissions-Policy", permissions);

        // Cache Control for sensitive pages
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            response.Headers.Append("Cache-Control", "no-store, no-cache, must-revalidate, max-age=0");
            response.Headers.Append("Pragma", "no-cache");
            response.Headers.Append("Expires", "0");
        }

        await _next(context);
    }
}

public static class SecurityHeadersMiddlewareExtensions
{
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SecurityHeadersMiddleware>();
    }
}
