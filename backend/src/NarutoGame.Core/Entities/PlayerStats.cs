namespace NarutoGame.Core.Entities;

public class PlayerStats
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public int Ninjutsu { get; set; } = 1;
    public int Taijutsu { get; set; } = 1;
    public int Genjutsu { get; set; } = 1;

    // Navigation property
    public Player? Player { get; set; }
}
