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
        // Remove Server header
        context.Response.Headers.Remove("Server");
        
        // Add security headers
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        context.Response.Headers["X-Frame-Options"] = "DENY";
        context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
        context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
        context.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
        
        // Content Security Policy (CSP)
        var cspPolicy = "default-src 'self'; " +
                       "script-src 'self' 'unsafe-inline' https://www.google-analytics.com; " +
                       "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; " +
                       "font-src 'self' https://fonts.gstatic.com; " +
                       "img-src 'self' data: https:; " +
                       "connect-src 'self' https://www.google-analytics.com; " +
                       "frame-ancestors 'none'; " +
                       "base-uri 'self'; " +
                       "form-action 'self'";
        
        context.Response.Headers["Content-Security-Policy"] = cspPolicy;
        
        // HSTS (only in production)
        if (!context.RequestServices.GetService<IHostEnvironment>()!.IsDevelopment())
        {
            context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload";
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
