using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Domain.Entities.Identity;
using Portfolio.Domain.Enums;
using Portfolio.Domain.Interfaces;
using Portfolio.Shared.DTOs.Auth;
using Portfolio.Shared.Exceptions;
using BCrypt.Net;

namespace Portfolio.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecurityLogService _securityLogService;

    private readonly int _accessTokenExpirationMinutes;
    private readonly int _refreshTokenExpirationDays;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _secretKey;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IConfiguration configuration,
        ILogger<AuthService> logger,
        IUnitOfWork unitOfWork,
        ISecurityLogService securityLogService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _securityLogService = securityLogService;

        var jwtSettings = _configuration.GetSection("JwtSettings");
        _accessTokenExpirationMinutes = int.Parse(jwtSettings["AccessTokenExpirationMinutes"] ?? "60");
        _refreshTokenExpirationDays = int.Parse(jwtSettings["RefreshTokenExpirationDays"] ?? "7");
        _issuer = jwtSettings["Issuer"] ?? "PortfolioCMS";
        _audience = jwtSettings["Audience"] ?? "PortfolioCMSUser";
        _secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key is missing.");
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, string ipAddress, string userAgent)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null)
        {
            await _securityLogService.LogSecurityEventAsync(SecurityEventType.FailedLogin, null, ipAddress, userAgent, $"Invalid email: {request.Email}");
            throw new UnauthorizedException("Geçersiz e-posta veya şifre.");
        }

        if (user.LockoutEnd != null && user.LockoutEnd > DateTime.UtcNow)
        {
            await _securityLogService.LogSecurityEventAsync(SecurityEventType.AccountLocked, user.Id, ipAddress, userAgent, "Account locked due to brute force.");
            throw new UnauthorizedException("Hesap çok fazla başarısız giriş denemesi nedeniyle kilitlendi.");
        }

        var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!passwordValid)
        {
            await _userManager.AccessFailedAsync(user);
            await _securityLogService.LogSecurityEventAsync(SecurityEventType.FailedLogin, user.Id, ipAddress, userAgent, "Invalid password attempt.");
            
            if (await _userManager.IsLockedOutAsync(user))
            {
                throw new UnauthorizedException("Hesap kilitlendi. Lütfen daha sonra deneyin.");
            }
            
            throw new UnauthorizedException("Geçersiz e-posta veya şifre.");
        }

        // Reset lockout on success
        await _userManager.ResetAccessFailedCountAsync(user);
        await _securityLogService.LogSecurityEventAsync(SecurityEventType.SuccessfulLogin, user.Id, ipAddress, userAgent, "Successful login.");

        var result = await GenerateTokensAndCreateResponseAsync(user, ipAddress, userAgent);
        
        // Save Refresh Token to DB
        user.RefreshToken = result.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);
        await _userManager.UpdateAsync(user);

        return result;
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, string ipAddress, string userAgent)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
        
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            await _securityLogService.LogSecurityEventAsync(SecurityEventType.InvalidRefreshToken, null, ipAddress, userAgent, "Invalid or expired refresh token.");
            throw new UnauthorizedException("Geçersiz veya süresi dolmuş refresh token.");
        }

        // Token Rotation Security: Invalidate old token immediately
        var oldToken = user.RefreshToken;
        var result = await GenerateTokensAndCreateResponseAsync(user, ipAddress, userAgent);
        
        user.RefreshToken = result.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);
        
        // Optional: Implement Refresh Token Reuse Detection (Store old token in blacklist briefly)
        
        await _userManager.UpdateAsync(user);
        return result;
    }

    public async Task<AuthResponseDto> RegisterSetupAdminAsync(SetupAdminRequestDto request, string ipAddress, string userAgent)
    {
        // Check if any user exists (Setup Wizard Guard)
        var anyUserExists = await _userManager.Users.AnyAsync();
        if (anyUserExists)
        {
            throw new BusinessException("Sistem zaten kurulmuş. İlk kullanıcı oluşturulamaz.");
        }

        var user = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email,
            FullName = request.FullName,
            EmailConfirmed = true, // Setup wizard bypasses email confirmation
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };

        // BCrypt Hashing
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, BCrypt.Net.BCrypt.GenerateSalt(12));
        
        var createUserResult = await _userManager.CreateAsync(user, hashedPassword); // Note: UserManager expects plain text usually, but we can override or hash manually if using custom provider. 
        // Correction: ASP.NET Identity expects plain text in CreateAsync, it hashes internally. 
        // However, requirement says BCrypt explicitly. We will use a custom PasswordHasher or just set the hash directly if bypassing Identity hasher.
        // To strictly follow BCrypt requirement while using Identity:
        user.PasswordHash = hashedPassword;
        await _userManager.UpdateAsync(user); 

        // Assign Super Admin Role
        if (!await _roleManager.RoleExistsAsync(RoleType.SuperAdmin.ToString()))
        {
            await _roleManager.CreateAsync(new ApplicationRole { Name = RoleType.SuperAdmin.ToString(), Description = "System Super Administrator" });
            await _roleManager.CreateAsync(new ApplicationRole { Name = RoleType.Admin.ToString(), Description = "Administrator" });
            await _roleManager.CreateAsync(new ApplicationRole { Name = RoleType.Editor.ToString(), Description = "Content Editor" });
            await _roleManager.CreateAsync(new ApplicationRole { Name = RoleType.Viewer.ToString(), Description = "Read Only User" });
        }

        await _userManager.AddToRoleAsync(user, RoleType.SuperAdmin.ToString());

        await _securityLogService.LogSecurityEventAsync(SecurityEventType.SetupCompleted, user.Id, ipAddress, userAgent, "Initial Super Admin created via Setup Wizard.");

        return await GenerateTokensAndCreateResponseAsync(user, ipAddress, userAgent);
    }

    private async Task<AuthResponseDto> GenerateTokensAndCreateResponseAsync(ApplicationUser user, string ipAddress, string userAgent)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Name, user.FullName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var accessToken = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes),
            signingCredentials: creds
        );

        var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        return new AuthResponseDto
        {
            AccessToken = accessTokenString,
            RefreshToken = refreshToken,
            Expiration = DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = roles.ToList()
            }
        };
    }

    public async Task LogoutAsync(string userId, string ipAddress, string userAgent)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            await _securityLogService.LogSecurityEventAsync(SecurityEventType.Logout, userId, ipAddress, userAgent, "User logged out.");
        }
    }
    
    public async Task<bool> IsSetupCompletedAsync()
    {
        return await _userManager.Users.AnyAsync();
    }
}
