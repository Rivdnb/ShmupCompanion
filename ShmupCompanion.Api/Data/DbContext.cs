using Microsoft.EntityFrameworkCore;
using ShmupCompanion.Api.Models;

namespace ShmupCompanion.Api.Data
{
    public class ShmupCompanionContext : DbContext
    {
        public ShmupCompanionContext(DbContextOptions<ShmupCompanionContext> options)
            : base(options) { }

        public DbSet<Game> Games => Set<Game>();
    }
}
