using Microsoft.EntityFrameworkCore;
using ShmupCompanion.Api.Models;

namespace ShmupCompanion.Api.Data
{
    public class ShmupCompanionContext : DbContext
    {
        public ShmupCompanionContext(DbContextOptions<ShmupCompanionContext> options)
            : base(options) { }

        public DbSet<Game> Games => Set<Game>();
        public DbSet<Run> Runs => Set<Run>();
        public DbSet<Note> Notes => Set<Note>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Game>()
                .HasMany(game => game.Runs)
                .WithOne(run => run.Game)
                .HasForeignKey(run => run.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Run>()
            .HasMany(run => run.Notes)
            .WithOne(note => note.Run)
            .HasForeignKey(note => note.RunId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
