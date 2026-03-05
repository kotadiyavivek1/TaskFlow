namespace TaskFlow.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }

    // Audit: who created this record
    public int CreatedBy { get; set; }
    public User CreatedByUser { get; set; } = null!;

    // Audit: who last updated this record (null if never updated)
    public int? UpdatedBy { get; set; }
    public User? UpdatedByUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}
