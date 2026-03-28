namespace NarutoGame.Core.Entities;

public class Mission
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int XpReward { get; set; }
    public int Difficulty { get; set; } = 1; // 1-5 stars
    public bool IsDaily { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation property
    public ICollection<PlayerMission>? PlayerMissions { get; set; }
}
