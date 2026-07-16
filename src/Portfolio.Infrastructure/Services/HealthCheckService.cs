using Microsoft.Extensions.Logging;
using Portfolio.Application.Interfaces;
using Portfolio.Persistence;

namespace Portfolio.Infrastructure.Services;

public class HealthCheckService : IHealthCheckService
{
    private readonly ApplicationDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly IFileStorageService _storageService;
    private readonly ILogger<HealthCheckService> _logger;

    public HealthCheckService(
        ApplicationDbContext context,
        ICacheService cacheService,
        IFileStorageService storageService,
        ILogger<HealthCheckService> logger)
    {
        _context = context;
        _cacheService = cacheService;
        _storageService = storageService;
        _logger = logger;
    }

    public async Task<HealthStatus> CheckHealthAsync(CancellationToken cancellationToken = default)
    {
        var status = new HealthStatus
        {
            IsHealthy = true,
            Status = "Healthy",
            CheckedAt = DateTime.UtcNow
        };

        try
        {
            status.Checks["Database"] = await CheckDatabaseComponentAsync(cancellationToken);
            status.Checks["Cache"] = await CheckCacheComponentAsync(cancellationToken);
            status.Checks["Storage"] = await CheckStorageComponentAsync(cancellationToken);

            status.IsHealthy = status.Checks.Values.All(c => c.IsHealthy);
            status.Status = status.IsHealthy ? "Healthy" : "Degraded";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sağlık kontrolü başarısız");
            status.IsHealthy = false;
            status.Status = "Unhealthy";
        }

        return status;
    }

    public async Task<bool> CheckDatabaseHealthAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await CheckDatabaseComponentAsync(cancellationToken);
            return result.IsHealthy;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> CheckCacheHealthAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await CheckCacheComponentAsync(cancellationToken);
            return result.IsHealthy;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> CheckStorageHealthAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await CheckStorageComponentAsync(cancellationToken);
            return result.IsHealthy;
        }
        catch
        {
            return false;
        }
    }

    private async Task<Application.Interfaces.HealthCheckResult> CheckDatabaseComponentAsync(CancellationToken cancellationToken)
    {
        var result = new Application.Interfaces.HealthCheckResult
        {
            Name = "Database",
            Description = "PostgreSQL veritabanı bağlantısı"
        };

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            await _context.Database.CanConnectAsync(cancellationToken);
            stopwatch.Stop();
            result.IsHealthy = true;
            result.ResponseTime = stopwatch.Elapsed;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            result.IsHealthy = false;
            result.ErrorMessage = ex.Message;
            result.ResponseTime = stopwatch.Elapsed;
        }

        return result;
    }

    private async Task<Application.Interfaces.HealthCheckResult> CheckCacheComponentAsync(CancellationToken cancellationToken)
    {
        var result = new Application.Interfaces.HealthCheckResult
        {
            Name = "Cache",
            Description = "Distributed cache servisi"
        };

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            var testKey = "healthcheck:test";
            await _cacheService.SetAsync(testKey, "test", TimeSpan.FromSeconds(10), cancellationToken);
            var value = await _cacheService.GetAsync<string>(testKey, cancellationToken);
            await _cacheService.RemoveAsync(testKey, cancellationToken);

            stopwatch.Stop();
            result.IsHealthy = value == "test";
            result.ResponseTime = stopwatch.Elapsed;
            if (!result.IsHealthy)
                result.ErrorMessage = "Cache read/write mismatch";
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            result.IsHealthy = false;
            result.ErrorMessage = ex.Message;
            result.ResponseTime = stopwatch.Elapsed;
        }

        return result;
    }

    private async Task<Application.Interfaces.HealthCheckResult> CheckStorageComponentAsync(CancellationToken cancellationToken)
    {
        var result = new Application.Interfaces.HealthCheckResult
        {
            Name = "Storage",
            Description = "Dosya depolama sistemi"
        };

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            // Basit bir dosya yazma/okuma testi
            var testContent = "healthcheck";
            var testFileName = $"healthcheck_{Guid.NewGuid()}.txt";
            
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(testContent));
            var filePath = await _storageService.UploadFileAsync(stream, testFileName, "text/plain", "healthcheck", cancellationToken);
            
            var exists = !string.IsNullOrEmpty(filePath);
            if (exists)
                await _storageService.DeleteFileAsync(filePath, cancellationToken);

            stopwatch.Stop();
            result.IsHealthy = exists;
            result.ResponseTime = stopwatch.Elapsed;
            if (!result.IsHealthy)
                result.ErrorMessage = "Storage write/read failed";
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            result.IsHealthy = false;
            result.ErrorMessage = ex.Message;
            result.ResponseTime = stopwatch.Elapsed;
        }

        return result;
    }
}
