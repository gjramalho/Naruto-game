namespace NarutoGame.Core.Entities;

public class Npc
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty; // naruto, sasuke, sakura, kakashi
    public string Name { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty; // Emoji
    public string Color { get; set; } = string.Empty; // CSS class color
    public string NpcType { get; set; } = string.Empty; // ally, enemy, neutral

    // Navigation property
    public ICollection<NpcDialogue>? Dialogues { get; set; }
}
