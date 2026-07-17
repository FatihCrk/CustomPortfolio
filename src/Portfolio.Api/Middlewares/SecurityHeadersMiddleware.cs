using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Portfolio.Api.Middlewares;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Content Security Policy
        context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data: https:; font-src 'self'; connect-src 'self' https:; frame-ancestors 'none';");
        
        // XSS Protection
        context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
        
        // Prevent MIME type sniffing
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        
        // Referrer Policy
        context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
        
        // Permissions Policy
        context.Response.Headers.Add("Permissions-Policy", "camera=(), microphone=(), geolocation=()");
        
        // Cache Control for sensitive pages
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate, max-age=0");
            context.Response.Headers.Add("Pragma", "no-cache");
            context.Response.Headers.Add("Expires", "0");
        }

        await _next(context);
    }
}
