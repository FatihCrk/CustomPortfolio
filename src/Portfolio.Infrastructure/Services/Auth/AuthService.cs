using Portfolio.Application.Common.Interfaces;
using Portfolio.Application.Common.Models;
using Portfolio.Domain.Entities;
using Portfolio.Infrastructure.Services.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Portfolio.Infrastructure.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IPortfolioDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(
        IPortfolioDbContext context,
        IConfiguration configuration,
        ILogger<AuthService> logger,
        IPasswordHasher passwordHasher)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> IsSetupCompletedAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users.AnyAsync(cancellationToken);
    }

    public async Task<(string accessToken, string refreshToken)> RegisterAsync(SetupWizardDto model, CancellationToken cancellationToken = default)
    {
        if (await IsSetupCompletedAsync(cancellationToken))
        {
            throw new InvalidOperationException("Setup already completed. Cannot register new user via setup wizard.");
        }

        // Check if username or email already exists
        if (await _context.Users.AnyAsync(u => u.Username == model.Username || u.Email == model.Email, cancellationToken))
        {
            throw new InvalidOperationException("Username or email already exists.");
        }

        var user = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Username = model.Username,
            Email = model.Email,
            PasswordHash = _passwordHasher.HashPassword(model.Password),
            IsActive = true,
            IsSuperAdmin = true,
            CreatedDate = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        // Create Super Admin role and assign
        var superAdminRole = new Role
        {
            Name = "SuperAdmin",
            Description = "System Super Administrator",
            IsSystemRole = true,
            CreatedDate = DateTime.UtcNow
        };

        _context.Roles.Add(superAdminRole);
        await _context.SaveChangesAsync(cancellationToken);

        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = superAdminRole.Id,
            CreatedDate = DateTime.UtcNow
        };

        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync(cancellationToken);

        // Generate tokens
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();

        // Save refresh token
        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresDate = DateTime.UtcNow.AddDays(7),
            CreatedDate = DateTime.UtcNow,
            IsRevoked = false
        };

        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Setup wizard completed. Super Admin user created: {Username}", user.Username);

        return (accessToken, refreshToken);
    }

    public async Task<(string accessToken, string refreshToken)> LoginAsync(string usernameOrEmail, string password, bool rememberMe = false, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => (u.Username == usernameOrEmail || u.Email == usernameOrEmail) && !u.IsDeleted, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("Login attempt with non-existent username/email: {UsernameOrEmail}", usernameOrEmail);
            throw new SecurityException("Invalid username or password.");
        }

        if (!user.IsActive)
        {
            throw new SecurityException("Account is deactivated.");
        }

        if (user.LockoutEndDate.HasValue && user.LockoutEndDate > DateTime.UtcNow)
        {
            throw new SecurityException($"Account is locked. Try again after {user.LockoutEndDate.Value}.");
        }

        if (!_passwordHasher.VerifyPassword(password, user.PasswordHash))
        {
            user.FailedLoginAttempts++;
            
            // Lock account after 5 failed attempts
            if (user.FailedLoginAttempts >= 5)
            {
                user.LockoutEndDate = DateTime.UtcNow.AddMinutes(15);
                _logger.LogWarning("Account locked due to multiple failed login attempts: {Username}", user.Username);
            }
            
            await _context.SaveChangesAsync(cancellationToken);
            throw new SecurityException("Invalid username or password.");
        }

        // Reset failed attempts on successful login
        user.FailedLoginAttempts = 0;
        user.LockoutEndDate = null;
        user.LastLoginDate = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        // Generate tokens
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();

        // Save refresh token
        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresDate = rememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddDays(7),
            CreatedDate = DateTime.UtcNow,
            IsRevoked = false,
            DeviceInfo = "Web Browser", // Can be enhanced with actual device info
            IpAddress = "" // Can be populated from HTTP context
        };

        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync(cancellationToken);

        // Log audit
        var auditLog = new AuditLog
        {
            UserId = user.Id,
            Action = "Login",
            EntityType = "User",
            EntityId = user.Id,
            IpAddress = "",
            UserAgent = "",
            CreatedDate = DateTime.UtcNow
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User logged in successfully: {Username}", user.Username);

        return (accessToken, refreshToken);
    }

    public async Task LogoutAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tokens = _context.RefreshTokens.Where(rt => rt.UserId == userId && !rt.IsRevoked);
        
        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.RevokedDate = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("User logged out: {UserId}", userId);
    }

    public async Task LogoutAllDevicesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await LogoutAsync(userId, cancellationToken);
        _logger.LogInformation("All devices logged out for user: {UserId}", userId);
    }

    public async Task<string> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var tokenEntity = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked && rt.ExpiresDate > DateTime.UtcNow, cancellationToken);

        if (tokenEntity == null)
        {
            throw new SecurityException("Invalid or expired refresh token.");
        }

        var user = await _context.Users.FindAsync(new object[] { tokenEntity.UserId }, cancellationToken);
        if (user == null || !user.IsActive)
        {
            throw new SecurityException("User not found or inactive.");
        }

        // Revoke old token (rotation)
        tokenEntity.IsRevoked = true;
        tokenEntity.RevokedDate = DateTime.UtcNow;

        // Generate new tokens
        var newAccessToken = GenerateAccessToken(user);
        var newRefreshToken = GenerateRefreshToken();

        // Save new refresh token
        var newRefreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            ExpiresDate = tokenEntity.ExpiresDate,
            CreatedDate = DateTime.UtcNow,
            IsRevoked = false
        };

        _context.RefreshTokens.Add(newRefreshTokenEntity);
        await _context.SaveChangesAsync(cancellationToken);

        return newRefreshToken;
    }

    public async Task<bool> RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var tokenEntity = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken, cancellationToken);
        
        if (tokenEntity != null)
        {
            tokenEntity.IsRevoked = true;
            tokenEntity.RevokedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        return false;
    }

    private string GenerateAccessToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured.");
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("Username", user.Username),
            new Claim("IsSuperAdmin", user.IsSuperAdmin.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Add roles
        var userRoles = _context.UserRoles
            .Where(ur => ur.UserId == user.Id)
            .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r)
            .ToList();

        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
