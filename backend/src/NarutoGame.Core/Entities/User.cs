namespace NarutoGame.Core.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Concurrency token for optimistic locking (SQL Server rowversion)
    /// </summary>
    public byte[]? RowVersion { get; set; }

    // Navigation property
    public Player? Player { get; set; }
}
