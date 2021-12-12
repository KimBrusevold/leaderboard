using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace leaderboard.Shared
{
    public class CreateLeaderboardEntry
    {
        public int VehicleId { get; set; }
        public int UserId { get; set; }
        public int TrackId { get; set; }
        public TimeSpan Time { get; set; }

    }
}
