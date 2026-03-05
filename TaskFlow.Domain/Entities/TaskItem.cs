using TaskFlow.Domain.Enums;
using TaskStatus = TaskFlow.Domain.Enums.TaskStatus;

namespace TaskFlow.Domain.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime DueDate { get; set; }

    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public int AssignedToId { get; set; }
    public User AssignedTo { get; set; } = null!;

    // Navigation
    public ICollection<TaskComment> Comments { get; set; } = [];
    public ICollection<TaskAttachment> Attachments { get; set; } = [];
    public ICollection<TaskHistory> Histories { get; set; } = [];
}
