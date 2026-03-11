namespace TaskFlow.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = null!;
    public DateTime Expires { get; set; }

    // Revocation
    public bool IsRevoked { get; set; }
    public DateTime RevokedAt { get; set; }
    public string? RevokedByIp { get; set; } = null!;
    public string? ReplacedByToken { get; set; }

    // IP tracking
    public string CreatedByIp { get; set; } = null!;

    // Status helper
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsActive   => !IsRevoked && !IsExpired;

    // Relations
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
