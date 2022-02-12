using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace leaderboard.Shared;
public class Entry
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    public string? Id { get; set; }
    public int? Rank { get; set; }
    public Vehicle? Vehicle { get; set; }
    public Track? Track { get; set; }
    public User? User { get; set; }
    public Game? Game { get; set; }
    public double Time { get; set; }
    public DateTime? Created { get; set; }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string? ToString()
    {
        var timeSpan = TimeSpan.FromSeconds(Time);
        return string.Format("{0}:{1}.{2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
    }
}
