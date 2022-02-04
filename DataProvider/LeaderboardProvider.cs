using leaderboard.DataProvider.Data;
using leaderboard.DataProvider.Interfaces;
using leaderboard.Shared;
using MongoDB.Driver;

namespace leaderboard.DataProvider;
public class LeaderboardProvider : ILeaderboardDataProvider
{
    private IMongoDatabase Database { get; init; }    
    private IEntryProvider Entries;
    public LeaderboardProvider(string connectionString)
    {
        var settings = MongoClientSettings.FromConnectionString(connectionString);

        var client = new MongoClient(settings);
        Database = client.GetDatabase("leaderboard");
        //"controllers"
        Entries = new Entries(Database);
    }

    IEntryProvider ILeaderboardDataProvider.Entries()
        => Entries;
}
