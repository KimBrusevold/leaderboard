using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace leaderboard.Shared
{
    public class User
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string? Id { get; set; }
        public string? DiscordId { get; set; }
        public string? UserName { get; set; }
        [BsonDefaultValue(false)]
        public bool? IsAdmin { get; set; }
        public string? ImageId { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
