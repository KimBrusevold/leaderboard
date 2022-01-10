using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace leaderboard.Shared
{
    public class Game
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Track> Tracks { get; set; }
        public List<Vehicle> Vehicles { get; set; }

        public override string? ToString()
        {
            return Name;
        }
    }

    
}
