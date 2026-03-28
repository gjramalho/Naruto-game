using Microsoft.EntityFrameworkCore;
using NarutoGame.Core.Entities;
using NarutoGame.Core.Interfaces;
using NarutoGame.Infrastructure.Data;

namespace NarutoGame.Infrastructure.Repositories;

public class NpcRepository : INpcRepository
{
    private readonly NarutoGameDbContext _context;

    public NpcRepository(NarutoGameDbContext context)
    {
        _context = context;
    }

    public async Task<Npc?> GetByIdAsync(Guid id)
    {
        return await _context.Npcs.FindAsync(id);
    }

    public async Task<Npc?> GetByKeyAsync(string key)
    {
        return await _context.Npcs
            .Include(n => n.Dialogues.OrderBy(d => d.Order))
            .FirstOrDefaultAsync(n => n.Key.ToLower() == key.ToLower());
    }

    public async Task<IEnumerable<Npc>> GetAllAsync()
    {
        return await _context.Npcs.ToListAsync();
    }

    public async Task<IEnumerable<Npc>> GetAllWithDialoguesAsync()
    {
        return await _context.Npcs
            .Include(n => n.Dialogues.OrderBy(d => d.Order))
            .ToListAsync();
    }

    public async Task<Npc> AddAsync(Npc npc)
    {
        _context.Npcs.Add(npc);
        await _context.SaveChangesAsync();
        return npc;
    }

    public async Task UpdateAsync(Npc npc)
    {
        _context.Npcs.Update(npc);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var npc = await GetByIdAsync(id);
        if (npc != null)
        {
            _context.Npcs.Remove(npc);
            await _context.SaveChangesAsync();
        }
    }
}
