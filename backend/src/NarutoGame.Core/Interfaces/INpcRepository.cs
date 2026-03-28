using NarutoGame.Core.Entities;

namespace NarutoGame.Core.Interfaces;

public interface INpcRepository
{
    Task<Npc?> GetByIdAsync(Guid id);
    Task<Npc?> GetByKeyAsync(string key);
    Task<IEnumerable<Npc>> GetAllAsync();
    Task<IEnumerable<Npc>> GetAllWithDialoguesAsync();
    Task<Npc> AddAsync(Npc npc);
    Task UpdateAsync(Npc npc);
    Task DeleteAsync(Guid id);
}
