using NarutoGame.Core.Enums;

namespace NarutoGame.Core.Entities;

public class Player
{
    public const int MAX_LEVEL = 100;
    
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; } = 1;
    public int Xp { get; set; } = 0;
    public int MaxXp { get; set; } = 100;
    public VillageType Village { get; set; } = VillageType.Undefined;
    public ElementType Element { get; set; } = ElementType.Undefined;
    public int Chakra { get; set; } = 100;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }

    // Navigation properties
    public User? User { get; set; }
    public PlayerStats? Stats { get; set; }
    public ICollection<PlayerMission>? CompletedMissions { get; set; }
}
