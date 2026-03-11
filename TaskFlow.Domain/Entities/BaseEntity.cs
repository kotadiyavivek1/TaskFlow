namespace TaskFlow.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }

    // Audit: who created this record (nullable — first user has no creator)
    public int? CreatedBy { get; set; }
    public User? CreatedByUser { get; set; }

    // Audit: who last updated this record
    public int? UpdatedBy { get; set; }
    public User? UpdatedByUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}
