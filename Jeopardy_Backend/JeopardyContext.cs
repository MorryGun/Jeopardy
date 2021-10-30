using Jeopardy_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Jeopardy_Backend
{
    public class JeopardyContext : DbContext
    {
        public JeopardyContext(DbContextOptions<JeopardyContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Player> Players { get; set; }

        public DbSet<Result> Results { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Competition> Competitions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Competition>()
                .Property(p => p.Date)
                .HasColumnType("Date");

            modelBuilder.Entity<Player>()
                .Property(p => p.Rate)
                .HasDefaultValue(1000);
        }
    }
}
