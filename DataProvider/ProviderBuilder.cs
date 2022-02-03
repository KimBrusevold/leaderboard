using leaderboard.DataProvider;
using leaderboard.DataProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider
{
    public static class ProviderBuilder
    {
        static ILeaderboardDataProvider CreateDataProvider(string connString)
            => new LeaderboardProvider(connString);
    }
}
