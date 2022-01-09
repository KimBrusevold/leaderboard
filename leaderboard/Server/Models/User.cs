using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace leaderboard.Server.Models
{
    public class User
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string DiscordId { get; set; }
        public string UserName { get; set; }
        [BsonDefaultValue(false)]
        public bool IsAdmin { get; set; }
        public string ImageId { get; set; }
    }
}
