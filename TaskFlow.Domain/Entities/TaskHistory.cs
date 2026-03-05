namespace TaskFlow.Domain.Entities;

public class TaskHistory
{
    public int Id { get; set; }
    public string Action { get; set; } = null!;
    public string PerformedBy { get; set; } = null!;
    public DateTime PerformedAt { get; set; }
    public string? Remarks { get; set; }
}
