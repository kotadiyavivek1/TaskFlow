using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Repositories.Interfaces;
using TaskFlow.Services.Services.Interfaces;
using TaskFlow.Shared.DTOs.User;
using TaskFlow.Shared.Settings;
using BC = BCrypt.Net.BCrypt;

namespace TaskFlow.Services.Services.Implementation;

public class AuthService(
    IGenericRepository<User>         userRepo,
    IGenericRepository<RefreshToken> tokenRepo,
    IGenericRepository<Role>         roleRepo,
    IGenericRepository<UserRole>     userRoleRepo,
    ITokenService                    tokenService,
    IOptions<JwtSettings>            jwtOptions) : IAuthService
{
    private readonly JwtSettings _jwt = jwtOptions.Value;

    // ─────────────────────────────────────── REGISTER ────────────────────────
    public async Task RegisterAsync(
        RegisterUserDto dto, string ipAddress)
    {
        var normalizedUserName = dto.UserName.ToLowerInvariant();
        var normalizedEmail    = dto.Email.ToLowerInvariant();

        if (await userRepo.ExistsAsync(u => u.UserName == normalizedUserName))
            throw new InvalidOperationException("Username is already taken.");

        if (await userRepo.ExistsAsync(u => u.Email == normalizedEmail))
            throw new InvalidOperationException("Email is already registered.");

        // Validate role exists
        var role = await roleRepo.GetByIdAsync(dto.RoleId)
            ?? throw new KeyNotFoundException($"Role with ID {dto.RoleId} does not exist.");

        // Create user (CreatedBy = null for self-registration)
        var user = new User
        {
            FullName     = dto.FullName,
            UserName     = normalizedUserName,
            Email        = normalizedEmail,
            PasswordHash = BC.HashPassword(dto.Password),
            PhoneNumber  = dto.PhoneNumber,
            IsActive     = true,
            CreatedAt    = DateTime.UtcNow,
        };

        await userRepo.AddAsync(user);

        var userRole = new UserRole
        {
            UserId    = user.Id,
            RoleId    = dto.RoleId,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = user.Id,
        };
        // Assign role
        await userRoleRepo.AddAsync(userRole);

        // Reload user with roles for token generation
        var userWithRoles = await GetUserWithRolesAsync(user.Id);
    }

    // ─────────────────────────────────────── LOGIN ───────────────────────────
    public async Task<(AuthResponseDto Auth, string RawRefreshToken)> LoginAsync(
        LoginDto dto, string ipAddress)
    {
        var normalized = dto.UserName.ToLowerInvariant();

        // Support login by either username or email
        var user = await userRepo.GetQueryable()
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.UserName == normalized || u.Email == normalized)
            ?? throw new UnauthorizedAccessException("Invalid username/email or password.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is disabled.");

        if (!BC.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid username/email or password.");

        return await IssueTokensAsync(user, ipAddress);
    }

    // ─────────────────────────────────────── REFRESH ─────────────────────────
    public async Task<(AuthResponseDto Auth, string RawRefreshToken)> RefreshTokenAsync(
        string refreshToken, string ipAddress)
    {
        var stored = await tokenRepo.GetQueryable()
            .Include(rt => rt.User)
                .ThenInclude(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken)
            ?? throw new UnauthorizedAccessException("Invalid refresh token.");

        if (!stored.IsActive)
            throw new UnauthorizedAccessException("Refresh token is no longer active.");

        var newRawToken = tokenService.GenerateRefreshToken();

        stored.IsRevoked       = true;
        stored.RevokedAt       = DateTime.UtcNow;
        stored.RevokedByIp     = ipAddress;
        stored.ReplacedByToken = newRawToken;
        await tokenRepo.UpdateAsync(stored);

        return await IssueTokensAsync(stored.User, ipAddress, newRawToken);
    }

    // ─────────────────────────────────────── REVOKE ──────────────────────────
    public async Task<bool> RevokeTokenAsync(string refreshToken, string ipAddress)
    {
        var stored = await tokenRepo.FindAsync(rt => rt.Token == refreshToken);
        if (stored is null || !stored.IsActive) return false;

        stored.IsRevoked   = true;
        stored.RevokedAt   = DateTime.UtcNow;
        stored.RevokedByIp = ipAddress;
        await tokenRepo.UpdateAsync(stored);
        return true;
    }

    // ─────────────────────────────────────── PRIVATE ─────────────────────────
    private async Task<User?> GetUserWithRolesAsync(int userId)
        => await userRepo.GetQueryable()
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);

    private async Task<(AuthResponseDto Auth, string RawRefreshToken)> IssueTokensAsync(
        User user, string ipAddress, string? preGeneratedToken = null)
    {
        var roles = user.UserRoles
            .Where(ur => ur.Role != null)
            .Select(ur => ur.Role.Name)
            .ToList();

        var accessToken = tokenService.GenerateAccessToken(user, roles);
        var rawRefresh  = preGeneratedToken ?? tokenService.GenerateRefreshToken();

        await tokenRepo.AddAsync(new RefreshToken
        {
            Token       = rawRefresh,
            UserId      = user.Id,
            Expires     = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpirationDays),
            IsRevoked   = false,
            CreatedByIp = ipAddress,
            CreatedBy   = user.Id,
            CreatedAt   = DateTime.UtcNow,
        });

        return (new AuthResponseDto
        {
            AccessToken = accessToken,
            ExpiresIn   = _jwt.AccessTokenExpirationMinutes * 60,
            UserId      = user.Id,
            FullName    = user.FullName,
            Email       = user.Email,
            Roles       = roles,
        }, rawRefresh);
    }
}
