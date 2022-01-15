using leaderboard.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger Logger;
        public GameController(IMongoDatabase database, ILogger<GameController> logger)
        {
            Database = database;
            Logger = logger;
        }
        // GET: api/<GameController>
        [HttpGet]
        public async Task<IEnumerable<Game>> Get()
        {
            var collection = Database.GetCollection<Game>(CollectionNames.GameCollection);



            var allFilter = FilterBuilder.Empty;
            var allGames = await collection.FindAsync(allFilter);
            var games = await allGames.ToListAsync();
            return games;
        }

        // GET api/<GameController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        //POST api/<GameController>
        // [HttpPost]
        // public async Task<IActionResult> Post()
        // {

        //     var collection = Database.GetCollection<Game>(CollectionNames.GameCollection);
        //     // var game = collection.Find(FilterBuilder.Empty).ToList().FirstOrDefault();

        //     var trackCollection = Database.GetCollection<Track>(CollectionNames.TrackCollection);
        //     var vehicleCollection = Database.GetCollection<Vehicle>(CollectionNames.VehicleCollection);
            
            
        //     var vehicles = await vehicleCollection.FindAsync(Builders<Vehicle>.Filter.Empty);
        //     var tracks = await trackCollection.FindAsync<Track>(Builders<Track>.Filter.Empty);

        //     var game = new Game
        //     {
        //         Tracks = await tracks.ToListAsync(),
        //         Vehicles = await vehicles.ToListAsync(),
        //         Name = "iRacing"
        //     };
        //     collection.InsertOne(game);
        //     return Ok();
        // }

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
