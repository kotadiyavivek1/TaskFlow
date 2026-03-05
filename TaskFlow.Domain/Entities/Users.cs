namespace TaskFlow.Domain.Entities;
public class User : BaseEntity
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;

    // ── Business navigations ──────────────────────────────────────────────────
    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<Project> OwnedProjects { get; set; } = [];
    public ICollection<TaskItem> AssignedTasks { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<TaskComment> TaskComments { get; set; } = [];

    // ── Audit back-navigations (CreatedBy / UpdatedBy from BaseEntity) ────────
    public ICollection<Project> CreatedProjects { get; set; } = [];
    public ICollection<Project> UpdatedProjects { get; set; } = [];

    public ICollection<Role> CreatedRoles { get; set; } = [];
    public ICollection<Role> UpdatedRoles { get; set; } = [];

    public ICollection<UserRole> CreatedUserRoles { get; set; } = [];
    public ICollection<UserRole> UpdatedUserRoles { get; set; } = [];

    public ICollection<TaskItem> CreatedTaskItems { get; set; } = [];
    public ICollection<TaskItem> UpdatedTaskItems { get; set; } = [];

    public ICollection<TaskComment> CreatedComments { get; set; } = [];
    public ICollection<TaskComment> UpdatedComments { get; set; } = [];

    public ICollection<TaskAttachment> CreatedAttachments { get; set; } = [];
    public ICollection<TaskAttachment> UpdatedAttachments { get; set; } = [];

    public ICollection<TaskHistory> CreatedHistories { get; set; } = [];
    public ICollection<TaskHistory> UpdatedHistories { get; set; } = [];

    public ICollection<RefreshToken> CreatedTokens { get; set; } = [];
    public ICollection<RefreshToken> UpdatedTokens { get; set; } = [];
}
