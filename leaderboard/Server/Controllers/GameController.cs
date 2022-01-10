using leaderboard.Shared;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace leaderboard.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IMongoDatabase Database;
        private static FilterDefinitionBuilder<Game> FilterBuilder = Builders<Game>.Filter;
        public GameController(IMongoDatabase database)
        {
            Database = database;
        }
        // GET: api/<GameController>
        [HttpGet]
        public async Task<IEnumerable<Game>> Get()
        {
            var collection = Database.GetCollection<Game>(CollectionNames.GameCollection);
            

            
            var allFilter = FilterBuilder.Empty;
            var allGames = await collection.FindAsync(allFilter);
            return await allGames.ToListAsync();
        }

        // GET api/<GameController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        //POST api/<GameController>
        [HttpPost]
        public async Task<IActionResult> Post()
        {

            var collection = Database.GetCollection<Game>(CollectionNames.GameCollection);
            var game = collection.Find(FilterBuilder.Empty).ToList().FirstOrDefault();

            var trackCollection = Database.GetCollection<Track>(CollectionNames.TrackCollection);
            var vehicleCollection = Database.GetCollection<Vehicle>(CollectionNames.VehicleCollection);
            var vehicles = vehicleCollection.Find(Builders<Vehicle>.Filter.Empty).ToList();

            var trackFilter = Builders<Track>.Filter.Where(t => t.Id == "61d49ceee516950adbc697c1");

            var tracks = await trackCollection.FindAsync<Track>(Builders<Track>.Filter.Empty);
            var track = await tracks.ToListAsync();

            game.Tracks = track;
            game.Vehicles = vehicles;

            collection.ReplaceOne(FilterBuilder.Eq("Id", game.Id), game);
            return Ok();
        }

        // PUT api/<GameController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GameController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
