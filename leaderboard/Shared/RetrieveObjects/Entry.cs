using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using leaderboard.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace leaderboard.Shared.RetrieveObjects;
public class Entry
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    public string Id { get; set; }
    public int? Rank {get; set;}
    public Vehicle Vehicle { get; set; }
    public Track Track { get; set; }
    public Shared.RetrieveObjects.User User {get; set;}
    public Game Game { get; set; }
    public int Minutes { get; set; }
    public int Seconds { get; set; }
    public int Thousands { get; set; }
}