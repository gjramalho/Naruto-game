using Microsoft.EntityFrameworkCore;
using NarutoGame.Core.Entities;
using NarutoGame.Core.Interfaces;
using NarutoGame.Infrastructure.Data;

namespace NarutoGame.Infrastructure.Repositories;

public class MissionRepository : IMissionRepository
{
    private readonly NarutoGameDbContext _context;

    public MissionRepository(NarutoGameDbContext context)
    {
        _context = context;
    }

    public async Task<Mission?> GetByIdAsync(Guid id)
    {
        return await _context.Missions.FindAsync(id);
    }

    public async Task<IEnumerable<Mission>> GetAllAsync()
    {
        return await _context.Missions
            .Where(m => m.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Mission>> GetDailyMissionsAsync()
    {
        return await _context.Missions
            .Where(m => m.IsActive && m.IsDaily)
            .ToListAsync();
    }

    public async Task<Mission> AddAsync(Mission mission)
    {
        _context.Missions.Add(mission);
        await _context.SaveChangesAsync();
        return mission;
    }

    public async Task UpdateAsync(Mission mission)
    {
        _context.Missions.Update(mission);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var mission = await GetByIdAsync(id);
        if (mission != null)
        {
            _context.Missions.Remove(mission);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<PlayerMission> CompleteMissionAsync(Guid playerId, Guid missionId)
    {
        var playerMission = new PlayerMission
        {
            Id = Guid.NewGuid(),
            PlayerId = playerId,
            MissionId = missionId,
            CompletedAt = DateTime.UtcNow,
            WasRewarded = true
        };

        _context.PlayerMissions.Add(playerMission);
        await _context.SaveChangesAsync();
        
        return playerMission;
    }

    public async Task<IEnumerable<PlayerMission>> GetCompletedMissionsAsync(Guid playerId)
    {
        return await _context.PlayerMissions
            .Include(pm => pm.Mission)
            .Where(pm => pm.PlayerId == playerId)
            .OrderByDescending(pm => pm.CompletedAt)
            .ToListAsync();
    }
}
