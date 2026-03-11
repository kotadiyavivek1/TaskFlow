using TaskFlow.Shared.DTOs.User;

namespace TaskFlow.Services.Services.Interfaces;

public interface IAuthService
{
    /// <summary>Register a new user. Returns access token DTO + raw refresh token string.</summary>
    Task RegisterAsync(RegisterUserDto dto, string? ipAddress);

    /// <summary>Login. Returns access token DTO + raw refresh token string.</summary>
    Task<(AuthResponseDto Auth, string RawRefreshToken)> LoginAsync(LoginDto dto, string? ipAddress);

    /// <summary>Rotate refresh token. Returns new access token DTO + new raw refresh token string.</summary>
    Task<(AuthResponseDto Auth, string RawRefreshToken)> RefreshTokenAsync(string refreshToken, string? ipAddress);

    /// <summary>Revoke a refresh token (logout). Returns false if not found / already revoked.</summary>
    Task<bool> RevokeTokenAsync(string refreshToken, string? ipAddress);
}
