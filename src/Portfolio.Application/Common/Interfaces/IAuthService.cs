using Portfolio.Domain.Entities;

namespace Portfolio.Application.Common.Interfaces;

public interface IAuthService
{
    Task<(string accessToken, string refreshToken)> RegisterAsync(SetupWizardDto model, CancellationToken cancellationToken = default);
    Task<(string accessToken, string refreshToken)> LoginAsync(string usernameOrEmail, string password, bool rememberMe = false, CancellationToken cancellationToken = default);
    Task LogoutAsync(Guid userId, CancellationToken cancellationToken = default);
    Task LogoutAllDevicesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<string> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<bool> RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<bool> IsSetupCompletedAsync(CancellationToken cancellationToken = default);
}
