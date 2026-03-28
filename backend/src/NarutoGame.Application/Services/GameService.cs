using AutoMapper;
using NarutoGame.Application.DTOs;
using NarutoGame.Core.Interfaces;

namespace NarutoGame.Application.Services;

public class GameService
{
    private readonly INpcRepository _npcRepository;
    private readonly IMissionRepository _missionRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IMapper _mapper;

    public GameService(
        INpcRepository npcRepository,
        IMissionRepository missionRepository,
        IPlayerRepository playerRepository,
        IMapper mapper)
    {
        _npcRepository = npcRepository;
        _missionRepository = missionRepository;
        _playerRepository = playerRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NpcDto>> GetAllNpcsAsync()
    {
        var npcs = await _npcRepository.GetAllWithDialoguesAsync();
        return _mapper.Map<IEnumerable<NpcDto>>(npcs);
    }

    public async Task<NpcDto?> GetNpcByKeyAsync(string key)
    {
        var npc = await _npcRepository.GetByKeyAsync(key);
        return npc != null ? _mapper.Map<NpcDto>(npc) : null;
    }

    public async Task<IEnumerable<MissionDto>> GetAllMissionsAsync()
    {
        var missions = await _missionRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<MissionDto>>(missions);
    }

    public async Task<IEnumerable<MissionDto>> GetDailyMissionsAsync()
    {
        var missions = await _missionRepository.GetDailyMissionsAsync();
        return _mapper.Map<IEnumerable<MissionDto>>(missions);
    }

    public async Task<CompleteMissionResponse> CompleteMissionAsync(Guid userId, Guid missionId)
    {
        var player = await _playerRepository.GetByUserIdAsync(userId);
        
        if (player == null)
        {
            return new CompleteMissionResponse
            {
                Success = false,
                Message = "Jogador não encontrado."
            };
        }

        var mission = await _missionRepository.GetByIdAsync(missionId);
        
        if (mission == null)
        {
            return new CompleteMissionResponse
            {
                Success = false,
                Message = "Missão não encontrada."
            };
        }

        // Complete the mission
        await _missionRepository.CompleteMissionAsync(player.Id, missionId);
        
        // Add XP reward
        var oldLevel = player.Level;
        await _playerRepository.AddXpAsync(player.Id, mission.XpReward);
        
        // Reload player
        player = await _playerRepository.GetByIdWithStatsAsync(player.Id);
        
        return new CompleteMissionResponse
        {
            Success = true,
            XpGained = mission.XpReward,
            CurrentXp = player!.Xp,
            CurrentLevel = player.Level,
            LeveledUp = player.Level > oldLevel,
            NewLevel = player.Level,
            Message = player.Level > oldLevel 
                ? $"Missão completada! +{mission.XpReward} XP e você subiu para o nível {player.Level}!" 
                : $"Missão completada! +{mission.XpReward} XP!"
        };
    }
}
