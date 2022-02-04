using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using leaderboard.DataProvider.Interfaces;
using leaderboard.Shared;
using MongoDB.Driver;

namespace leaderboard.DataProvider.Data
{
    public class Entries : IEntryProvider
    {
        private IMongoDatabase Database { get; init; }
        public Entries(IMongoDatabase database)
        {
            Database = database;
        }

        public async Task<List<Entry>> All()
            =>  await GetFromCollection<Entry>(DBCollectionNames.EntryCollection).ToListAsync();

         public async Task<List<Entry>> Filter(string gameId)
    {
        var filter = Builders<Entry>.Filter.Eq(ent => ent.Game.Id, gameId);
        var entries = await GetFromCollection<Entry>(DBCollectionNames.EntryCollection, filter)
            .SortByDescending(entry=> entry.Time)
            .ToListAsync();

        if(entries == null || entries.Count() <= 0)
            return null;

        return SortByBestTimePerUser(entries);
    }

    public async Task<List<Entry>> Filter(string gameId, string trackId)
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

    public async Task<List<Entry>> Filter(string gameId, string trackId, string categoryId)
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

    

    private List<Entry> SortByBestTimePerUser(in IEnumerable<Entry> entries)
    {
        if(entries is null)
            return null;

        IEnumerable<IGrouping<string, Entry>> userGroup = entries.GroupBy(ent => ent.User.Id);
        List<Entry> bestPerPlayerList = new (userGroup.Count());
        
        foreach (var userEntries in userGroup)
        {            
            if(string.IsNullOrWhiteSpace(userEntries.Key))
                continue;

            bestPerPlayerList.Add(userEntries.First());
        }
        return bestPerPlayerList;
    }
        
        /// <summary>
        /// With just a collectionName, gets every object in collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private IFindFluent<T, T> GetFromCollection<T>(in string collectionName, FilterDefinition<T>? filter = null)
        {
            var collection = Database.GetCollection<T>(collectionName);

            if (filter is null)
                filter = Builders<T>.Filter.Empty;

            return collection.Find(filter);
        }
    }
}