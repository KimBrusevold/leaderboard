using DatabaseActions;
using MongoDB.Driver;

var settings = MongoClientSettings.FromConnectionString("");
var client = new MongoClient(settings);




var database = client.GetDatabase("leaderboard");

var collection = database.GetCollection<User>("users");

User user = new()
{
    UserName = "Kim",
};

//await collection.InsertOneAsync(user);


var trackCol = database.GetCollection<Track>("tracks");

var track = new Track()
{
    Name = "Circuit de Barcelona-Catalunya"
};

//await trackCol.InsertOneAsync(track);

var barcaFilter = Builders<Track>.Filter.Eq("Name", "Circuit de Barcelona-Catalunya");
var barca = await trackCol.Find(barcaFilter).FirstAsync();


var timeCol = database.GetCollection<TrackTime>("track_times");

var trackT = new TrackTime()
{
    Created = DateTime.UtcNow,
    Time = new TimeSpan(0, 0, 2, 24, 100),
    Track = barca,
    User = user,
};

//await timeCol.InsertOneAsync(trackT);


var filter = Builders<TrackTime>.Filter.Where(t => t.Track.Id == barca.Id && t.User.Id == null);



var res = await timeCol.Find(filter).SortBy(t => t.Time).ToListAsync();

foreach (var item in res)
{
    Console.WriteLine(item.Time);
}