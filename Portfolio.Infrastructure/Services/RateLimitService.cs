using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Portfolio.Application.Services.Interfaces;
using System.Collections.Concurrent;

namespace Portfolio.Infrastructure.Services;

public class RateLimitService : IRateLimitService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RateLimitService> _logger;
    private readonly ConcurrentDictionary<string, RateLimitInfo> _inMemoryCache;

    public RateLimitService(IDistributedCache cache, ILogger<RateLimitService> logger)
    {
        _cache = cache;
        _logger = logger;
        _inMemoryCache = new ConcurrentDictionary<string, RateLimitInfo>();
    }

    public async Task<bool> IsAllowedAsync(string key, int limit, TimeSpan timeWindow, CancellationToken cancellationToken = default)
    {
        try
        {
            var cacheKey = $"ratelimit:{key}:{timeWindow.TotalMinutes}m";
            
            // Try to get from distributed cache first
            var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);
            
            RateLimitInfo info;
            
            if (!string.IsNullOrEmpty(cached))
            {
                info = System.Text.Json.JsonSerializer.Deserialize<RateLimitInfo>(cached)!;
            }
            else
            {
                // Fallback to in-memory cache
                if (!_inMemoryCache.TryGetValue(cacheKey, out info!))
                {
                    info = new RateLimitInfo
                    {
                        Count = 0,
                        WindowStart = DateTime.UtcNow
                    };
                }
            }

            // Check if window has expired
            if (DateTime.UtcNow - info.WindowStart > timeWindow)
            {
                info = new RateLimitInfo
                {
                    Count = 1,
                    WindowStart = DateTime.UtcNow
                };
                
                await SaveToCacheAsync(cacheKey, info, timeWindow, cancellationToken);
                return true;
            }

            // Increment count
            info.Count++;

            // Check if limit exceeded
            if (info.Count > limit)
            {
                _logger.LogWarning("Rate limit exceeded for key: {Key}. Count: {Count}, Limit: {Limit}", 
                    key, info.Count, limit);
                
                await SaveToCacheAsync(cacheKey, info, timeWindow, cancellationToken);
                return false;
            }

            await SaveToCacheAsync(cacheKey, info, timeWindow, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking rate limit for key: {Key}", key);
            // Fail open - allow request if there's an error
            return true;
        }
    }

    public async Task<RateLimitInfo?> GetInfoAsync(string key, TimeSpan timeWindow, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"ratelimit:{key}:{timeWindow.TotalMinutes}m";
        
        var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);
        
        if (!string.IsNullOrEmpty(cached))
        {
            return System.Text.Json.JsonSerializer.Deserialize<RateLimitInfo>(cached);
        }

        if (_inMemoryCache.TryGetValue(cacheKey, out var info))
        {
            return info;
        }

        return null;
    }

    public async Task ResetAsync(string key, TimeSpan timeWindow, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"ratelimit:{key}:{timeWindow.TotalMinutes}m";
        
        await _cache.RemoveAsync(cacheKey, cancellationToken);
        _inMemoryCache.TryRemove(cacheKey, out _);
        
        _logger.LogInformation("Rate limit reset for key: {Key}", key);
    }

    private async Task SaveToCacheAsync(string cacheKey, RateLimitInfo info, TimeSpan timeWindow, CancellationToken cancellationToken)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(info);
        var remainingTime = timeWindow - (DateTime.UtcNow - info.WindowStart);
        
        if (remainingTime > TimeSpan.Zero)
        {
            await _cache.SetStringAsync(cacheKey, json, new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = remainingTime
            }, cancellationToken);
        }

        // Also save to in-memory cache as fallback
        _inMemoryCache.AddOrUpdate(cacheKey, info, (_, _) => info);
    }
}

public class RateLimitInfo
{
    public int Count { get; set; }
    public DateTime WindowStart { get; set; }
}

public interface IRateLimitService
{
    Task<bool> IsAllowedAsync(string key, int limit, TimeSpan timeWindow, CancellationToken cancellationToken = default);
    Task<RateLimitInfo?> GetInfoAsync(string key, TimeSpan timeWindow, CancellationToken cancellationToken = default);
    Task ResetAsync(string key, TimeSpan timeWindow, CancellationToken cancellationToken = default);
}
