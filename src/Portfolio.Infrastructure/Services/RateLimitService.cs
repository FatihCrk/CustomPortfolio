using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Portfolio.Application.Interfaces;

namespace Portfolio.Infrastructure.Services;

public class RateLimitService : IRateLimitService
{
    private readonly IDistributedCache _cache;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public RateLimitService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<bool> IsAllowedAsync(string identifier, int limit, TimeSpan window, CancellationToken cancellationToken = default)
    {
        var key = $"ratelimit:{identifier}:{window.TotalMinutes}";
        var data = await _cache.GetStringAsync(key, cancellationToken);

        var entries = data != null 
            ? JsonSerializer.Deserialize<List<RateLimitEntry>>(data, _jsonOptions) ?? new List<RateLimitEntry>()
            : new List<RateLimitEntry>();

        var now = DateTime.UtcNow;
        var windowStart = now - window;

        // Pencere dışındaki kayıtları temizle
        entries = entries.Where(e => e.Timestamp > windowStart).ToList();

        if (entries.Count >= limit)
            return false;

        // Yeni kayıt ekle
        entries.Add(new RateLimitEntry { Timestamp = now });

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = window
        };

        var serialized = JsonSerializer.Serialize(entries, _jsonOptions);
        await _cache.SetStringAsync(key, serialized, options, cancellationToken);

        return true;
    }

    public async Task<int> GetRemainingRequestsAsync(string identifier, int limit, TimeSpan window, CancellationToken cancellationToken = default)
    {
        var key = $"ratelimit:{identifier}:{window.TotalMinutes}";
        var data = await _cache.GetStringAsync(key, cancellationToken);

        var entries = data != null
            ? JsonSerializer.Deserialize<List<RateLimitEntry>>(data, _jsonOptions) ?? new List<RateLimitEntry>()
            : new List<RateLimitEntry>();

        var windowStart = DateTime.UtcNow - window;
        entries = entries.Where(e => e.Timestamp > windowStart).ToList();

        return Math.Max(0, limit - entries.Count);
    }

    public async Task ResetAsync(string identifier, CancellationToken cancellationToken = default)
    {
        var key = $"ratelimit:{identifier}";
        await _cache.RemoveAsync(key, cancellationToken);
    }

    private class RateLimitEntry
    {
        public DateTime Timestamp { get; set; }
    }
}
