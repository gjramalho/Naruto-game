namespace NarutoGame.Core.Entities;

public class NpcDialogue
{
    public Guid Id { get; set; }
    public Guid NpcId { get; set; }
    public string Text { get; set; } = string.Empty;
    public int Order { get; set; }

    // Navigation property
    public Npc? Npc { get; set; }
}
