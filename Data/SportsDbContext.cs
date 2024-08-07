using Lab5NET.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab5NET.Data
{
    public class SportsDbContext : DbContext
    {
        public SportsDbContext(DbContextOptions<SportsDbContext> options) : base(options)
        {
        }

        // DbSet properties for entity objects
        public DbSet<Fan> Fans { get; set; }
        public DbSet<SportClub> SportClubs { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Prediction> Predictions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fan>().ToTable("Fan");
            modelBuilder.Entity<SportClub>().ToTable("SportClub");
            modelBuilder.Entity<Subscription>().ToTable("Subscription");
            modelBuilder.Entity<Prediction>().ToTable("Prediction");

            // Configure composite primary key for Subscription entity
            modelBuilder.Entity<Subscription>()
                .HasKey(s => new { s.FanId, s.SportClubId });

            // Configure relationships
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Fan)
                .WithMany(f => f.Subscriptions)
                .HasForeignKey(s => s.FanId);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.SportClub)
                .WithMany(sc => sc.Subscriptions)
                .HasForeignKey(s => s.SportClubId);

            modelBuilder.Entity<Prediction>()
                .HasOne(p => p.SportClub)
                .WithMany(s => s.Predictions)
                .HasForeignKey(p => p.SportClubId);
        }
    }
}
