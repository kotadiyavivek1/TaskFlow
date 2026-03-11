using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Shared.DTOs.User;

public class RegisterUserDto
{
    [Required]
    public string FullName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [RegularExpression(@"^(?=.*\d)[a-zA-Z0-9_]+$",
    ErrorMessage = "Username must contain at least one number and can only include letters, numbers, and underscores.")]
    
    public string UserName { get; set; } = null!;
    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, number, and special character.")]
    public string Password { get; set; } = null!;

    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = null!;

    [Required]
    [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number format.")]
    public string PhoneNumber { get; set; } = null!;
    [Required]
    public int RoleId { get; set; }
}
