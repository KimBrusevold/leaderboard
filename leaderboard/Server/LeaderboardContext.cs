using leaderboard.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace leaderboard.Server
{
    public class LeaderboardContext : DbContext
    {
        public DbSet<LeaderboardEntry> LeaderboardEntries { get; set; }
        public DbSet<Track> Tracks{ get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<User> Users { get; set; }

        public LeaderboardContext(DbContextOptions<LeaderboardContext> options)
            :base (options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LeaderboardEntry>().ToTable("LeaderboardEntry");
            modelBuilder.Entity<Track>().ToTable("Track");
            modelBuilder.Entity<Vehicle>().ToTable("Vehicle");
            modelBuilder.Entity<User>().ToTable("User");
        }
    }

    

    public class Track
    {
        [Key]
        public int TrackId { get; set; }
        public string Name { get; set; }
    }

    
}
