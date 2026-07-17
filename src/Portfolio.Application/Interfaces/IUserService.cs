using Portfolio.Shared.DTOs.Users;

namespace Portfolio.Application.Interfaces;

/// <summary>
/// Kullanıcı yönetimi servisi arayüzü.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// ID ile kullanıcı getirir.
    /// </summary>
    Task<UserResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Username ile kullanıcı getirir.
    /// </summary>
    Task<UserResponseDto?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Email ile kullanıcı getirir.
    /// </summary>
    Task<UserResponseDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sayfalı kullanıcı listesi getirir.
    /// </summary>
    Task<(IEnumerable<UserListDto> Items, int TotalCount)> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? searchTerm = null,
        Guid? roleId = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Yeni kullanıcı oluşturur.
    /// </summary>
    Task<UserResponseDto> CreateAsync(UserCreateDto request, Guid createdBy, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Kullanıcıyı günceller.
    /// </summary>
    Task<UserResponseDto> UpdateAsync(Guid id, UserUpdateDto request, Guid updatedBy, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Kullanıcıyı siler (soft delete).
    /// </summary>
    Task DeleteAsync(Guid id, Guid deletedBy, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Kullanıcı durumunu değiştirir (aktif/pasif).
    /// </summary>
    Task ToggleStatusAsync(Guid id, Guid changedBy, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Kullanıcı şifresini değiştirir.
    /// </summary>
    Task ChangePasswordAsync(Guid userId, ChangeUserPasswordDto request, Guid changedBy, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Kullanıcının rollerini günceller.
    /// </summary>
    Task UpdateRolesAsync(Guid userId, List<Guid> roleIds, Guid updatedBy, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Username'in benzersiz olup olmadığını kontrol eder.
    /// </summary>
    Task<bool> IsUsernameUniqueAsync(string username, Guid? excludeId = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Email'in benzersiz olup olmadığını kontrol eder.
    /// </summary>
    Task<bool> IsEmailUniqueAsync(string email, Guid? excludeId = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Kullanıcı başarısız giriş denemesini kaydeder.
    /// </summary>
    Task RecordFailedLoginAsync(Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Kullanıcı başarılı girişini kaydeder.
    /// </summary>
    Task RecordSuccessfulLoginAsync(Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Kullanıcı kilidini kaldırır.
    /// </summary>
    Task UnlockAccountAsync(Guid userId, CancellationToken cancellationToken = default);
}
