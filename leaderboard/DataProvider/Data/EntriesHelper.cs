using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using leaderboard.DataProvider.Interfaces;
using leaderboard.Shared;
using MongoDB.Driver;

namespace leaderboard.DataProvider.Data;
public partial class Entries : IEntryProvider
{
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

    private void AddUserFilter(ref FilterDefinition<Entry> filter, in string userId)
    {
        var userFilter = Builders<Entry>.Filter.Eq(ent => ent.User.Id, userId);
        filter = Builders<Entry>.Filter.And(filter, userFilter);
    }
}
