using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TaskFlow.Services.Services.Interfaces;
using TaskFlow.Shared.DTOs.User;
using TaskFlow.Shared.Settings;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    IAuthService authService,
    IOptions<JwtSettings> jwtOptions) : ControllerBase
{
    private readonly JwtSettings _jwt = jwtOptions.Value;

    private string? IpAddress =>
        Request.Headers.TryGetValue("X-Forwarded-For", out var fwd)
            ? fwd.ToString()
            : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

    // ─────────────────────── POST /api/auth/register ─────────────────────────
    /// <summary>Register a new user. Returns an access token; refresh token is set as HttpOnly cookie.</summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        await authService.RegisterAsync(dto, IpAddress);
        return Ok(new { message = "User Registered successfully"});
    }

    // ─────────────────────── POST /api/auth/login ────────────────────────────
    /// <summary>Login. Returns an access token in body + sets refresh token as HttpOnly cookie.</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var (auth, rawRefresh) = await authService.LoginAsync(dto, IpAddress);
        AppendRefreshCookie(rawRefresh);
        return Ok(auth);
    }

    // ─────────────────────── POST /api/auth/refresh-token ────────────────────
    /// <summary>
    /// Exchange the HttpOnly refresh-token cookie for a new access token + rotated refresh cookie.
    /// No request body required — token is read from the cookie.
    /// </summary>
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var token = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(token))
            return BadRequest(new { message = "Refresh token is missing." });

        var (auth, rawRefresh) = await authService.RefreshTokenAsync(token, IpAddress);
        AppendRefreshCookie(rawRefresh);
        return Ok(auth);
    }

    // ─────────────────────── POST /api/auth/revoke-token ─────────────────────
    /// <summary>Logout — revokes the current refresh token and clears the cookie.</summary>
    [Authorize]
    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken()
    {
        var token = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(token))
            return BadRequest(new { message = "Refresh token is missing." });

        var revoked = await authService.RevokeTokenAsync(token, IpAddress);
        if (!revoked)
            return BadRequest(new { message = "Token is invalid or already revoked." });

        Response.Cookies.Delete("refreshToken");
        return Ok(new { message = "Logged out successfully." });
    }

    // ─────────────────────── GET /api/auth/me ────────────────────────────────
    /// <summary>Returns the current authenticated user's identity from the JWT claims.</summary>
    [Authorize]
    [HttpGet("current-user")]
    public IActionResult CurrentUser()
    {
        return Ok(new
        {
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            email  = User.FindFirstValue(ClaimTypes.Email),
            name   = User.FindFirstValue(ClaimTypes.Name),
            roles  = User.FindAll(ClaimTypes.Role).Select(c => c.Value),
        });
    }

    // ─────────────────────── PRIVATE ─────────────────────────────────────────
    private void AppendRefreshCookie(string rawRefreshToken)
    {
        Response.Cookies.Append("refreshToken", rawRefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure   = true,                      // Enforce HTTPS in production
            SameSite = SameSiteMode.Strict,
            Expires  = DateTimeOffset.UtcNow.AddDays(_jwt.RefreshTokenExpirationDays),
        });
    }
}
