using System.ComponentModel.DataAnnotations;

namespace leaderboard.Server.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }
        public string Name { get; set; }
    }
}
