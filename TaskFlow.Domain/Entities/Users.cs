namespace TaskFlow.Domain.Entities;
public class User : BaseEntity
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;
    // Navigation
    public ICollection<UserRole> UserRoles { get; set; } = null!;
    public ICollection<Project> OwnedProjects { get; set; } = null!;
    public ICollection<TaskItem> AssignedTasks { get; set; } = null!;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = null!;
}
