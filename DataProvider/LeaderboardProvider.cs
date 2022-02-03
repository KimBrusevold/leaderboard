using leaderboard.DataProvider.Interfaces;
using leaderboard.Shared;
using MongoDB.Driver;

namespace leaderboard.DataProvider;
public class LeaderboardProvider : ILeaderboardDataProvider
{
    private IMongoDatabase Database { get; init; }    
    
    public LeaderboardProvider(string connectionString)
    {
        var settings = MongoClientSettings.FromConnectionString(connectionString);

        var client = new MongoClient(settings);
        Database = client.GetDatabase("leaderboard");

    }

    public async Task<List<Entry>> GetAllEntries()
        =>  await GetFromCollection<Entry>(DBCollectionNames.EntryCollection);
    
    public async Task<List<Entry>> GetEntries(string gameId)
    {
        var filter = Builders<Entry>.Filter.Eq(ent => ent.Game.Id, gameId);
        return await GetFromCollection<Entry>(DBCollectionNames.EntryCollection, filter);
    }

    /// <summary>
    /// With just a collectionName, gets every object in collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collectionName"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    private Task<List<T>> GetFromCollection<T>(in string collectionName, FilterDefinition<T>? filter = null)
    {
        var collection = Database.GetCollection<T>(collectionName);

        if (filter is null)
            filter = Builders<T>.Filter.Empty;

        return collection.Find(filter)
                .SortBy(ent => ent.Time)
                .ToListAsync();
    }

    
}
