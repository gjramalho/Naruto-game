using NarutoGame.Core.Entities;

namespace NarutoGame.Core.Interfaces;

public interface IMissionRepository
{
    Task<Mission?> GetByIdAsync(Guid id);
    Task<IEnumerable<Mission>> GetAllAsync();
    Task<IEnumerable<Mission>> GetDailyMissionsAsync();
    Task<Mission> AddAsync(Mission mission);
    Task UpdateAsync(Mission mission);
    Task DeleteAsync(Guid id);
    Task<PlayerMission> CompleteMissionAsync(Guid playerId, Guid missionId);
    Task<IEnumerable<PlayerMission>> GetCompletedMissionsAsync(Guid playerId);
}
