using System.ComponentModel.DataAnnotations;

namespace leaderboard.Server.Models
{
    public class LeaderboardEntry
    {
        [Key]
        public int EntryId { get; set; }
        public TimeSpan Time { get; set; }
        public User User { get; set; }
        public Vehicle Vehicle { get; set; }
        public Track Track { get; set; }
    }
}
