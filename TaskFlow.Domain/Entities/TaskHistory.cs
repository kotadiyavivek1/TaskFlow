namespace TaskFlow.Domain.Entities;

/// <summary>
/// Tracks a meaningful state-change event on a TaskItem.
/// "Who performed the action" is captured by the inherited <see cref="BaseEntity.CreatedBy"/> FK.
/// "When it happened" is captured by the inherited <see cref="BaseEntity.CreatedAt"/>.
/// </summary>
public class TaskHistory : BaseEntity
{
    /// <summary>Short label for the change, e.g. "Status changed to InProgress".</summary>
    public string Action { get; set; } = null!;

    /// <summary>Optional extra detail / reason for the change.</summary>
    public string? Remarks { get; set; }

    /// <summary>The task this history entry belongs to.</summary>
    public int TaskItemId { get; set; }
    public TaskItem TaskItem { get; set; } = null!;
}
