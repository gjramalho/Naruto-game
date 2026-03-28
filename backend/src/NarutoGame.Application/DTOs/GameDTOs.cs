namespace NarutoGame.Application.DTOs;

public class NpcDto
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string NpcType { get; set; } = string.Empty;
    public List<string> Dialogues { get; set; } = new();
}

public class MissionDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int XpReward { get; set; }
    public int Difficulty { get; set; }
    public bool IsDaily { get; set; }
}

public class CompleteMissionRequest
{
    public Guid MissionId { get; set; }
}

public class CompleteMissionResponse
{
    public bool Success { get; set; }
    public int XpGained { get; set; }
    public int CurrentXp { get; set; }
    public int CurrentLevel { get; set; }
    public bool LeveledUp { get; set; }
    public int NewLevel { get; set; }
    public string? Message { get; set; }
}
