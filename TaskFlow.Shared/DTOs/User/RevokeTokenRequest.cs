namespace TaskFlow.Shared.DTOs.User;

/// <summary>Used by the revoke-token endpoint when a cookie is not available (e.g. Swagger).</summary>
public class RevokeTokenRequest
{
    public string Token { get; set; } = null!;
}
