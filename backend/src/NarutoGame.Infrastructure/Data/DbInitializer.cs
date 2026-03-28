using Microsoft.EntityFrameworkCore;
using NarutoGame.Core.Entities;

namespace NarutoGame.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(NarutoGameDbContext context)
    {
        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Seed NPCs if empty
        if (!context.Npcs.Any())
        {
            var npcs = SeedData.GetDefaultNpcs();
            await context.Npcs.AddRangeAsync(npcs);
        }

        // Seed Missions if empty
        if (!context.Missions.Any())
        {
            var missions = SeedData.GetDefaultMissions();
            await context.Missions.AddRangeAsync(missions);
        }

        await context.SaveChangesAsync();
    }
}
