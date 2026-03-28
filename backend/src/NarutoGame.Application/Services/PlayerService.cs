using AutoMapper;
using NarutoGame.Application.DTOs;
using NarutoGame.Core.Entities;
using NarutoGame.Core.Enums;
using NarutoGame.Core.Interfaces;

namespace NarutoGame.Application.Services;

public class PlayerService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IMapper _mapper;

    public PlayerService(IPlayerRepository playerRepository, IMapper mapper)
    {
        _playerRepository = playerRepository;
        _mapper = mapper;
    }

    public async Task<PlayerDto?> GetPlayerByUserIdAsync(Guid userId)
    {
        var player = await _playerRepository.GetByUserIdAsync(userId);
        return player != null ? _mapper.Map<PlayerDto>(player) : null;
    }

    public async Task<PlayerDto?> GetPlayerByIdAsync(Guid playerId)
    {
        var player = await _playerRepository.GetByIdWithStatsAsync(playerId);
        return player != null ? _mapper.Map<PlayerDto>(player) : null;
    }

    public async Task<(bool Success, PlayerDto? Player, string? Error)> UpdatePlayerAsync(
        Guid userId, UpdatePlayerRequest request)
    {
        var player = await _playerRepository.GetByUserIdAsync(userId);
        
        if (player == null)
        {
            return (false, null, "Jogador não encontrado.");
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            player.Name = request.Name;
        }

        if (!string.IsNullOrWhiteSpace(request.Village))
        {
            if (Enum.TryParse<VillageType>(request.Village, true, out var village))
            {
                player.Village = village;
            }
        }

        if (!string.IsNullOrWhiteSpace(request.Element))
        {
            if (Enum.TryParse<ElementType>(request.Element, true, out var element))
            {
                player.Element = element;
            }
        }

        await _playerRepository.UpdateAsync(player);
        
        return (true, _mapper.Map<PlayerDto>(player), null);
    }

    public async Task<AddXpResponse> AddXpAsync(Guid userId, int amount)
    {
        // SECURITY: Validar amount para prevenir exploits
        // O valor deve ser positivo (> 0) e não exceder o limite máximo (10000)
        if (amount <= 0)
        {
            return new AddXpResponse
            {
                Success = false,
                Message = "Valor de XP inválido. O valor deve ser maior que 0."
            };
        }

        const int MAX_XP_AMOUNT = 10000;
        if (amount > MAX_XP_AMOUNT)
        {
            return new AddXpResponse
            {
                Success = false,
                Message = $"Valor de XP inválido. O valor máximo permitido é {MAX_XP_AMOUNT}."
            };
        }

        var player = await _playerRepository.GetByUserIdAsync(userId);
        
        if (player == null)
        {
            return new AddXpResponse
            {
                Success = false,
                Message = "Jogador não encontrado."
            };
        }

        // Check if player is already at max level
        if (player.Level >= Player.MAX_LEVEL)
        {
            return new AddXpResponse
            {
                Success = false,
                Message = $"Você já está no nível máximo ({Player.MAX_LEVEL})!"
            };
        }

        var oldLevel = player.Level;
        
        await _playerRepository.AddXpAsync(player.Id, amount);
        
        // Reload player to get updated values
        player = await _playerRepository.GetByIdWithStatsAsync(player.Id);
        
        return new AddXpResponse
        {
            Success = true,
            CurrentXp = player!.Xp,
            CurrentLevel = player.Level,
            MaxXp = player.MaxXp,
            LeveledUp = player.Level > oldLevel,
            NewLevel = player.Level,
            Message = player.Level > oldLevel 
                ? $"Parabéns! Você subiu para o nível {player.Level}!" 
                : $"+{amount} XP!"
        };
    }

    public async Task<(bool Success, string? Error)> UpdateSkillsAsync(
        Guid userId, UpdateSkillsRequest request)
    {
        var player = await _playerRepository.GetByUserIdAsync(userId);
        
        if (player == null)
        {
            return (false, "Jogador não encontrado.");
        }

        // Validar limites das habilidades para prevenir exploits
        const int MIN_STAT = 1;
        const int MAX_STAT = 100;
        
        var ninjutsu = Math.Clamp(request.Ninjutsu ?? 1, MIN_STAT, MAX_STAT);
        var taijutsu = Math.Clamp(request.Taijutsu ?? 1, MIN_STAT, MAX_STAT);
        var genjutsu = Math.Clamp(request.Genjutsu ?? 1, MIN_STAT, MAX_STAT);

        var stats = new PlayerStats
        {
            Ninjutsu = ninjutsu,
            Taijutsu = taijutsu,
            Genjutsu = genjutsu
        };

        await _playerRepository.UpdateSkillsAsync(player.Id, stats);
        
        return (true, null);
    }
}
