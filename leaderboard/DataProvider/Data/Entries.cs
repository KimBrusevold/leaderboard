using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using leaderboard.DataProvider.Exceptions;
using leaderboard.DataProvider.Interfaces;
using leaderboard.Shared;
using MongoDB.Driver;

namespace leaderboard.DataProvider.Data;
public partial class Entries : IEntryProvider
{
    private ILeaderboardDataProvider Providers { get; init; }
    private IMongoDatabase Database { get; init; }
    public Entries(IMongoDatabase database, ILeaderboardDataProvider providers)
    {
        Database = database;
        Providers = providers;
    }

    public async Task<List<Entry>> All(string? userId = null)
    {
        FilterDefinition<Entry> filter = null;
        if(string.IsNullOrWhiteSpace(userId) is false)
            filter = Builders<Entry>.Filter.Eq(entry => entry.User.Id, userId);

        return await GetFromCollection<Entry>(DBCollectionNames.EntryCollection, filter).ToListAsync();
    }
        

        public async Task<List<Entry>> Filter(string gameId, string? userId = null)
    {
        var filter = Builders<Entry>.Filter.Eq(ent => ent.Game.Id, gameId);

        if(userId is not null)
            AddUserFilter(ref filter, in userId);

        var entries = await GetFromCollection<Entry>(DBCollectionNames.EntryCollection, filter)
            .SortByDescending(entry=> entry.Time)
            .ToListAsync();

        if(entries == null || entries.Count() <= 0)
            return null;

        return SortByBestTimePerUser(entries);
    }

    public async Task<List<Entry>> Filter(string gameId, string trackId, string? userId = null)
    {
        var filter = Builders<Entry>.Filter.Eq(ent => ent.Game.Id, gameId);
        var trackFilter = Builders<Entry>.Filter.Eq(ent=> ent.Track.Id, trackId);

        var gameAndTrackFilter = Builders<Entry>.Filter.And(filter, trackFilter);

        var entries = await GetFromCollection<Entry>(DBCollectionNames.EntryCollection, gameAndTrackFilter)
            .SortBy(ent => ent.Time)
            .ToListAsync();

        var sorted = SortByBestTimePerUser(entries);

        return sorted;
    }

    public async Task<List<Entry>> Filter(string gameId, string trackId, string categoryId, string? userId = null)
    {
        var filter = Builders<Entry>.Filter.Eq(ent => ent.Game.Id, gameId);
        var trackFilter = Builders<Entry>.Filter.Eq(ent=> ent.Track.Id, trackId);
        var categoryFilter = Builders<Entry>.Filter.Eq(ent=> ent.Vehicle.Category.Id, categoryId);

        var gameAndTrackFilter = Builders<Entry>.Filter.And(filter, trackFilter, categoryFilter);

        var entries = await GetFromCollection<Entry>(DBCollectionNames.EntryCollection, gameAndTrackFilter)
            .SortBy(ent => ent.Time)
            .ToListAsync();

        var sorted = SortByBestTimePerUser(entries);

        return sorted;
    }

    public Task Create(in Entry entry)
    {
        var user = Providers.Users().Find(entry.User.Id);

        if(user is null)
            throw new NotFoundException($"User with ID: {entry.User.Id} was not found!!!");


        throw new NotImplementedException();        
    }
}
