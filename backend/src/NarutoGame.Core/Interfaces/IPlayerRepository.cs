using NarutoGame.Core.Entities;

namespace NarutoGame.Core.Interfaces;

public interface IPlayerRepository
{
    Task<Player?> GetByIdAsync(Guid id);
    Task<Player?> GetByUserIdAsync(Guid userId);
    Task<Player?> GetByIdWithStatsAsync(Guid id);
    Task<IEnumerable<Player>> GetAllAsync();
    Task<Player> AddAsync(Player player);
    Task UpdateAsync(Player player);
    Task DeleteAsync(Guid id);
    Task AddXpAsync(Guid playerId, int xp);
    Task UpdateLevelAsync(Guid playerId, int newLevel);
    Task UpdateSkillsAsync(Guid playerId, PlayerStats stats);
}
