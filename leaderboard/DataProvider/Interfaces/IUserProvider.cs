using leaderboard.Shared;

namespace leaderboard.DataProvider.Interfaces
{
    public interface IUserProvider
    {
        public Task<List<User>> All();
        public Task<User> Find(string userId);
        public Task<User> GetByDiscrodId(string? discordId);
    }
}