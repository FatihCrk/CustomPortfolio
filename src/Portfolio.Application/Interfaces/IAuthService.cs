namespace Portfolio.Application.Interfaces;

/// <summary>
/// Kimlik doğrulama servisi arayüzü.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Kullanıcı girişi yapar.
    /// </summary>
    Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kayıt olur (Setup Wizard için).
    /// </summary>
    Task<AuthResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refresh token ile yeni access token alır.
    /// </summary>
    Task<AuthResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Çıkış yapar.
    /// </summary>
    Task LogoutAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tüm cihazlardan çıkış yapar.
    /// </summary>
    Task LogoutAllDevicesAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Şifre sıfırlama isteği oluşturur.
    /// </summary>
    Task ForgotPasswordAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Şifre sıfırlar.
    /// </summary>
    Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Şifre değiştirir.
    /// </summary>
    Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Token'ı blacklist'e ekler.
    /// </summary>
    Task RevokeTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// 2FA doğrulaması yapar.
    /// </summary>
    Task<bool> VerifyTwoFactorAsync(Guid userId, string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// 2FA kurulumu için secret key oluşturur.
    /// </summary>
    Task<TwoFactorSetupResult> SetupTwoFactorAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 2FA'yı etkinleştirir.
    /// </summary>
    Task EnableTwoFactorAsync(Guid userId, string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// 2FA'yı devre dışı bırakır.
    /// </summary>
    Task DisableTwoFactorAsync(Guid userId, string code, CancellationToken cancellationToken = default);
}

/// <summary>
/// Giriş isteği modeli.
/// </summary>
public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; } = false;
    public string? TwoFactorCode { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}

/// <summary>
/// Kayıt isteği modeli.
/// </summary>
public class RegisterRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Şifre sıfırlama isteği modeli.
/// </summary>
public class ResetPasswordRequest
{
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

/// <summary>
/// Şifre değiştirme isteği modeli.
/// </summary>
public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

/// <summary>
/// Kimlik doğrulama sonucu.
/// </summary>
public class AuthResult
{
    public bool Success { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string? Message { get; set; }
    public UserDto? User { get; set; }
    public bool RequiresTwoFactor { get; set; } = false;
    public string? TwoFactorProvider { get; set; }
}

/// <summary>
/// Kullanıcı DTO.
/// </summary>
public class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public ICollection<string> Roles { get; set; } = new List<string>();
    public bool IsTwoFactorEnabled { get; set; }
}

/// <summary>
/// 2FA kurulum sonucu.
/// </summary>
public class TwoFactorSetupResult
{
    public string SecretKey { get; set; } = string.Empty;
    public string ManualEntryKey { get; set; } = string.Empty;
    public string QrCodeSetupUri { get; set; } = string.Empty;
}
