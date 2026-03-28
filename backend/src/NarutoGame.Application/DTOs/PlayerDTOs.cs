using System.ComponentModel.DataAnnotations;
using NarutoGame.Core.Enums;

namespace NarutoGame.Application.DTOs;

public class PlayerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public int Xp { get; set; }
    public int MaxXp { get; set; }
    public string Village { get; set; } = string.Empty;
    public string Element { get; set; } = string.Empty;
    public int Chakra { get; set; }
    public PlayerStatsDto? Stats { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PlayerStatsDto
{
    public int Ninjutsu { get; set; }
    public int Taijutsu { get; set; }
    public int Genjutsu { get; set; }
}

public class UpdatePlayerRequest
{
    [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres.")]
    public string? Name { get; set; }

    public string? Village { get; set; }
    public string? Element { get; set; }
}

public class UpdateSkillsRequest
{
    [Range(1, 100, ErrorMessage = "Ninjutsu deve estar entre 1 e 100.")]
    public int? Ninjutsu { get; set; }

    [Range(1, 100, ErrorMessage = "Taijutsu deve estar entre 1 e 100.")]
    public int? Taijutsu { get; set; }

    [Range(1, 100, ErrorMessage = "Genjutsu deve estar entre 1 e 100.")]
    public int? Genjutsu { get; set; }
}

public class AddXpRequest
{
    [Required(ErrorMessage = "Quantidade de XP é obrigatória.")]
    [Range(1, 10000, ErrorMessage = "Quantidade de XP deve estar entre 1 e 10000.")]
    public int Amount { get; set; }
}

public class AddXpResponse
{
    public bool Success { get; set; }
    public int CurrentXp { get; set; }
    public int CurrentLevel { get; set; }
    public int MaxXp { get; set; }
    public bool LeveledUp { get; set; }
    public int NewLevel { get; set; }
    public string? Message { get; set; }
}
