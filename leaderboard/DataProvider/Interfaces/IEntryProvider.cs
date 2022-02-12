using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using leaderboard.Shared;

namespace leaderboard.DataProvider.Interfaces
{
    public interface IEntryProvider
    {
        public Task<List<Entry>> All(string? userId = null);
        public Task<List<Entry>> Filter(string gameId, string? userId = null);
        public Task<List<Entry>> Filter(string gameId, string trackId, string? userId = null);
        public Task<List<Entry>> Filter(string gameId, string trackId, string categoryId, string? userId = null);
        public Task Create(in Entry entry);
    }
}