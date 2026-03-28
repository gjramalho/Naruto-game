using Microsoft.EntityFrameworkCore;
using NarutoGame.Core.Entities;

namespace NarutoGame.Infrastructure.Data;

public class NarutoGameDbContext : DbContext
{
    public NarutoGameDbContext(DbContextOptions<NarutoGameDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Player> Players => Set<Player>();
    public DbSet<PlayerStats> PlayerStats => Set<PlayerStats>();
    public DbSet<Npc> Npcs => Set<Npc>();
    public DbSet<NpcDialogue> NpcDialogues => Set<NpcDialogue>();
    public DbSet<Mission> Missions => Set<Mission>();
    public DbSet<PlayerMission> PlayerMissions => Set<PlayerMission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            entity.Property(u => u.Nickname).IsRequired().HasMaxLength(50);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.Nickname).IsUnique();
            
            // RowVersion for optimistic concurrency (SQL Server rowversion/timestamp)
            entity.Property(u => u.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        // Player configuration
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.HasOne(p => p.User)
                .WithOne(u => u.Player)
                .HasForeignKey<Player>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(p => p.Stats)
                .WithOne(s => s.Player)
                .HasForeignKey<PlayerStats>(s => s.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // PlayerStats configuration
        modelBuilder.Entity<PlayerStats>(entity =>
        {
            entity.HasKey(s => s.Id);
        });

        // Npc configuration
        modelBuilder.Entity<Npc>(entity =>
        {
            entity.HasKey(n => n.Id);
            entity.Property(n => n.Name).IsRequired().HasMaxLength(100);
            entity.Property(n => n.Key).IsRequired().HasMaxLength(50);
            entity.HasIndex(n => n.Key).IsUnique();
        });

        // NpcDialogue configuration
        modelBuilder.Entity<NpcDialogue>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.HasOne(d => d.Npc)
                .WithMany(n => n.Dialogues)
                .HasForeignKey(d => d.NpcId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Mission configuration
        modelBuilder.Entity<Mission>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Title).IsRequired().HasMaxLength(200);
            entity.Property(m => m.Description).HasMaxLength(1000);
        });

        // PlayerMission configuration
        modelBuilder.Entity<PlayerMission>(entity =>
        {
            entity.HasKey(pm => pm.Id);
            entity.HasOne(pm => pm.Player)
                .WithMany(p => p.CompletedMissions)
                .HasForeignKey(pm => pm.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(pm => pm.Mission)
                .WithMany(m => m.PlayerMissions)
                .HasForeignKey(pm => pm.MissionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
