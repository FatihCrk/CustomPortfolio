using Portfolio.Application.DTOs;

namespace Portfolio.Application.Interfaces;

public interface IProjectService
{
    Task<PagedResult<ProjectDto>> GetProjectsAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        int? categoryId,
        bool? isFeatured,
        string sortBy,
        bool sortDescending);
    
    Task<ProjectDto?> GetProjectByIdAsync(int id);
    Task<ProjectDto?> GetProjectBySlugAsync(string slug);
    Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto, int userId);
    Task<ProjectDto?> UpdateProjectAsync(UpdateProjectDto dto, int userId);
    Task<bool> DeleteProjectAsync(int id, int userId);
    Task<ProjectDto?> ToggleFeaturedAsync(int id, int userId);
}

public interface IMessageService
{
    Task<ApiResponse> CreateMessageAsync(CreateContactMessageDto dto, string ipAddress, string userAgent);
    Task<PagedResult<ContactMessageDto>> GetMessagesAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        string? status,
        string sortBy,
        bool sortDescending);
    
    Task<ContactMessageDto?> GetMessageByIdAsync(int id, int userId);
    Task<ContactMessageDto?> UpdateMessageStatusAsync(UpdateMessageStatusDto dto, int userId);
    Task<bool> MarkAsReadAsync(int id, int userId);
    Task<bool> ArchiveMessageAsync(int id, int userId);
    Task<bool> DeleteMessageAsync(int id, int userId);
    Task<string> ExportToCsvAsync(string? status, DateTime? startDate, DateTime? endDate);
    Task<object> GetStatsAsync();
}

public interface IAuthService
{
    Task<ApiResponse<AuthResponse>> LoginAsync(string usernameOrEmail, string password, string ipAddress, string userAgent, bool rememberMe);
    Task<ApiResponse<AuthResponse>> RefreshTokenAsync(string refreshToken, string ipAddress, string userAgent);
    Task RevokeTokenAsync(string refreshToken, int userId);
    Task RevokeAllTokensAsync(int userId);
    Task<ApiResponse<AuthResponse>> SetupAdminAsync(SetupAdminRequest request, string ipAddress, string userAgent);
    Task<ApiResponse> ForgotPasswordAsync(string email);
    Task<ApiResponse> ResetPasswordAsync(string token, string newPassword);
    Task<ApiResponse> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    Task<UserInfo?> GetUserByIdAsync(int userId);
}

public interface IFileStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string folder);
    Task<bool> DeleteFileAsync(string filePath);
    Task<bool> IsValidFileTypeAsync(IFormFile file);
    Task<bool> IsValidFileSizeAsync(IFormFile file, long maxSizeInBytes);
}

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    Task RemoveAsync(string key);
    Task RemoveByPatternAsync(string pattern);
}

public interface ISlugService
{
    string GenerateSlug(string text);
    Task<string> GenerateUniqueSlugAsync(string text, string entityType);
}

public interface IRateLimitService
{
    bool IsAllowed(string identifier, int limit, TimeSpan timeWindow);
    Task<(bool Allowed, TimeSpan? RetryAfter)> CheckRateLimitAsync(string identifier, int limit, TimeSpan timeWindow);
}

public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string body);
    Task<bool> SendPasswordResetEmailAsync(string email, string resetToken);
    Task<bool> SendContactNotificationAsync(string contactName, string contactEmail, string subject, string message);
}

public interface IHealthCheckService
{
    Task<bool> CheckDatabaseHealthAsync();
    Task<bool> CheckCacheHealthAsync();
    Task<bool> CheckStorageHealthAsync();
    Task<Dictionary<string, bool>> CheckAllAsync();
}
