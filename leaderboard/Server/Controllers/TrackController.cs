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

        public TrackController(IMongoDatabase database)
        {
            Database = database;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IEnumerable<Track>> Get()
        {
            var collection = Database.GetCollection<Track>(CollectionNames.TrackCollection);
            var tracks = await collection.FindAsync(new BsonDocument());
            
            return await tracks.ToListAsync();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
