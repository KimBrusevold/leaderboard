using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using leaderboard.Shared;

namespace leaderboard.DataProvider.Interfaces
{
    public interface IEntryProvider
    {
        public Task<List<Entry>> All();
        public Task<List<Entry>> Filter(string gameId);
        public Task<List<Entry>> Filter(string gameId, string trackId);
        public Task<List<Entry>> Filter(string gameId, string trackId, string categoryId);
        
    }
}