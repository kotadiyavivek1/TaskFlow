namespace TaskFlow.Shared.DTOs.User;

public class AuthResponseDto
{
    public string AccessToken { get; set; } = null!;
    public int ExpiresIn { get; set; }      // seconds
    public int UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public IEnumerable<string> Roles { get; set; } = [];
}
