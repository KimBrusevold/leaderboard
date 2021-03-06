using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseActions
{
    public class User
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string DiscordId { get; set; }
        public string UserName { get; set; }
        [BsonDefaultValue(false)]
        public bool IsAdmin { get; set; } 
    }

    public class TrackTime
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public User User { get; set; }
        public DateTime Created { get; set; }
        public TimeSpan Time { get; set; }
        public Track Track { get; set; }
        public Vehicle Vehicle { get; set; }
        public Game Game { get; set; }
    }
    public class Track
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class Vehicle
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class Game
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
