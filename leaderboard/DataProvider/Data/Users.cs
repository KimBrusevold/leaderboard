using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using leaderboard.DataProvider.Interfaces;
using leaderboard.Shared;
using MongoDB.Driver;

namespace leaderboard.DataProvider.Data
{
    public class Users : IUserProvider
    {
        private IMongoDatabase Database { get; init; }

        public Users(IMongoDatabase database)
        {
            Database = database;
        }

        public async Task<List<Shared.User>?> All()
        {
            var userFilter = Builders<User>.Filter.Empty;
            var users = await FilterUsers(userFilter).ToListAsync();
            return users;
        }

        public async Task<User> Find(string userId)
        {
            if(string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("userId cannot be null or empty");

            var userFilter = Builders<User>.Filter.Eq(user => user.Id, userId);
            var user = await FilterUsers(userFilter).FirstOrDefaultAsync();

            return user;
        }

        public async Task<User> GetByDiscrodId(string? discordId)
        {
            var userFilter = Builders<User>.Filter.Eq(user => user.DiscordId, discordId);
            
            var user = await FilterUsers(userFilter).FirstOrDefaultAsync();

            return user;
        }

        public IFindFluent<User, User> FilterUsers(in FilterDefinition<User> filter, in ProjectionDefinition<User, User> projection = null)
        {
            var userCol = Database.GetCollection<User>(DBCollectionNames.UserCollection);        
            var userFind =  userCol.Find(filter);

            if(projection is null)
                return userFind;

            return userFind.Project(projection);
        }

        
    }
}