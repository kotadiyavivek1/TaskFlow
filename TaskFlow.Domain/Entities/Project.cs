namespace TaskFlow.Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    // Navigation
    public ICollection<TaskItem> Tasks { get; set; } = null!;
}
