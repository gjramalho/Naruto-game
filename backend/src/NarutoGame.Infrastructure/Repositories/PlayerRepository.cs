using Microsoft.EntityFrameworkCore;
using NarutoGame.Core.Entities;
using NarutoGame.Core.Interfaces;
using NarutoGame.Infrastructure.Data;

namespace NarutoGame.Infrastructure.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly NarutoGameDbContext _context;

    public PlayerRepository(NarutoGameDbContext context)
    {
        _context = context;
    }

    public async Task<Player?> GetByIdAsync(Guid id)
    {
        return await _context.Players.FindAsync(id);
    }

    public async Task<Player?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Players
            .Include(p => p.Stats)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<Player?> GetByIdWithStatsAsync(Guid id)
    {
        return await _context.Players
            .Include(p => p.Stats)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Player>> GetAllAsync()
    {
        return await _context.Players
            .Include(p => p.Stats)
            .ToListAsync();
    }

    public async Task<Player> AddAsync(Player player)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync();
        return player;
    }

    public async Task UpdateAsync(Player player)
    {
        _context.Players.Update(player);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var player = await GetByIdAsync(id);
        if (player != null)
        {
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddXpAsync(Guid playerId, int xp)
    {
        var player = await GetByIdWithStatsAsync(playerId);
        if (player != null)
        {
            player.Xp += xp;
            
            // Check for level up
            while (player.Xp >= player.MaxXp)
            {
                player.Xp -= player.MaxXp;
                player.Level++;
                player.MaxXp = CalculateMaxXp(player.Level);
            }
            
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateLevelAsync(Guid playerId, int newLevel)
    {
        var player = await GetByIdAsync(playerId);
        if (player != null)
        {
            player.Level = newLevel;
            player.MaxXp = CalculateMaxXp(newLevel);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateSkillsAsync(Guid playerId, PlayerStats stats)
    {
        var existingStats = await _context.PlayerStats
            .FirstOrDefaultAsync(s => s.PlayerId == playerId);
        
        if (existingStats != null)
        {
            existingStats.Ninjutsu = stats.Ninjutsu;
            existingStats.Taijutsu = stats.Taijutsu;
            existingStats.Genjutsu = stats.Genjutsu;
            await _context.SaveChangesAsync();
        }
    }

    private static int CalculateMaxXp(int level)
    {
        // XP required grows with level
        return 100 + (level * 50);
    }
}
