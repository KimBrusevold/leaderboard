using leaderboard.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace leaderboard.DataProvider.Interfaces;

public interface ILeaderboardDataProvider
{    
    public IEntryProvider Entries();
}