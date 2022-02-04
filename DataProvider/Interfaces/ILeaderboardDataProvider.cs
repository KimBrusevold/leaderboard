using leaderboard.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace leaderboard.DataProvider.Interfaces;

public interface ILeaderboardDataProvider
{
    public Task<List<Entry>> GetAllEntries();
    public Task<List<Entry>> GetEntries(string gameId);
    public Task<List<Entry>> GetEntries(string gameId, string trackId);
    public Task<List<Entry>> GetEntries(string gameId, string trackId, string categoryId);

}