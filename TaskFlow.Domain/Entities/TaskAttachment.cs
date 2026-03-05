namespace TaskFlow.Domain.Entities;

public class TaskAttachment : BaseEntity
{
    public string FileName { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public string? ContentType { get; set; }

    public int TaskItemId { get; set; }
    public TaskItem TaskItem { get; set; } = null!;
}
