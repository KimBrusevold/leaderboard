using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace leaderboard.Shared
{
    public class baseDBObject
    {
        
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public DateTime? Created {get; set;}
    }
}