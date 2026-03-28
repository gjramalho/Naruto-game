namespace NarutoGame.Core.Entities;

public class PlayerMission
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid MissionId { get; set; }
    public DateTime CompletedAt { get; set; }
    public bool WasRewarded { get; set; }

    // Navigation properties
    public Player? Player { get; set; }
    public Mission? Mission { get; set; }
}
