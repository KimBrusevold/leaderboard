using System.ComponentModel.DataAnnotations;

namespace leaderboard.Server.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
