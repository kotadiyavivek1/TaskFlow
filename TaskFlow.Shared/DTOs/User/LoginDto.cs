using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Shared.DTOs.User;

public class LoginDto
{
    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
