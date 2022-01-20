using Ganss.XSS;
using leaderboard.Shared;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace leaderboard.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly IMongoDatabase Database;
        private HtmlSanitizer Sanitizer = new HtmlSanitizer();

        public TrackController(IMongoDatabase database)
        {
            Database = database;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IEnumerable<Track>> Get(string? gameId, string? name)
        {    
            var collection = Database.GetCollection<Track>(CollectionNames.TrackCollection);
            List<Track> trackList;
            
            if(string.IsNullOrWhiteSpace(gameId) is false)
            {
                var gameCollection = Database.GetCollection<Game>(CollectionNames.GameCollection);
                trackList = await gameCollection.Find(ga => ga.Id == gameId).Project(ga => ga.Tracks).FirstOrDefaultAsync();
            }
            else
            {
                trackList = await collection.Find(new BsonDocument()).ToListAsync();
            }

            if(string.IsNullOrWhiteSpace(name) is false)
            {
                return trackList.Where(t => t.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase)).Take(10);
            }

            return trackList;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        // [HttpPost]
        // public async Task Post([FromBody] Track track)
        // {
        //     var trackCol = Database.GetCollection<Track>(CollectionNames.TrackCollection);

        //     track.Id = null;
        //     track.Name = Sanitizer.Sanitize(track.Name);
        //     await trackCol.InsertOneAsync(track);

        // }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        // [HttpDelete]
        // public void Delete()
        // {
        //     var trackCol = Database.GetCollection<Track>(CollectionNames.TrackCollection);
        //     trackCol.DeleteMany(Builders<Track>.Filter.Empty);
        // }
    }
}
