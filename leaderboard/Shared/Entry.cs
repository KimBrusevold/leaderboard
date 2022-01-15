using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace leaderboard.Shared
{
    public class Entry
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public Vehicle Vehicle { get; set; }
        public Track Track { get; set; }
        public User User { get; set; }
        public Game Game { get; set; }
        public TimeSpan Time { get; set; }
    }
}
