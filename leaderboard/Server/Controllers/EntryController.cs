using leaderboard.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace leaderboard.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private readonly IMongoDatabase Database;
        IMongoCollection<Shared.RetrieveObjects.Entry> EntryCollection;
        private static FilterDefinitionBuilder<Shared.RetrieveObjects.Entry> FilterBuilder = Builders<Shared.RetrieveObjects.Entry>.Filter;


        public EntryController(IMongoDatabase mongoDatabase)
        {
            Database = mongoDatabase;
            EntryCollection = Database.GetCollection<Shared.RetrieveObjects.Entry>(CollectionNames.EntryCollection);
        }


        // GET: api/<EntryController>
        [HttpGet("{id}")]
        public async Task <IEnumerable<Shared.RetrieveObjects.Entry>> Get(string id)
        {

            var filter = FilterBuilder.Eq(x => x.Track.Id, id);
            var entries = await EntryCollection.Find(filter)
                .SortBy(e => e.Minutes).ThenBy(e => e.Seconds).ThenBy(e => e.Thousands)
                .ToListAsync();
            var userGroup = entries.GroupBy(ent => ent.User.Id);

            var bestEntries = new List<Shared.RetrieveObjects.Entry>(userGroup.Count());
            
            var rank = 1;
            foreach (var ent in userGroup)
            {
                var entry = ent.First();
                entry.Rank = rank;
                rank++;
                bestEntries.Add(entry);
            }

        

            return bestEntries;
        }

        // GET api/<EntryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<EntryController>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Shared.Createobjects.Entry value)
        {
            var userCollection = Database.GetCollection<Shared.RetrieveObjects.User>(CollectionNames.UserCollection);

            var userBuilder = Builders<Shared.RetrieveObjects.User>.Filter;

            var filter = userBuilder.Eq("DiscordId", value.User.DiscrodId);
            var user = await userCollection.Find(filter).FirstOrDefaultAsync();

            if(user is null)
                return BadRequest("Did not find a user");

            try
            {

                var entry = new Shared.RetrieveObjects.Entry()
                {
                    Track = value.Track,
                    Vehicle = value.Vehicle,
                    User = new Shared.RetrieveObjects.User{
                        Id = user.Id,
                        UserName = user.UserName
                    },
                    Game = new Game {
                        Id = value.Game.Id,
                        Name = value.Game.Name
                    },
                    Minutes = value.Minutes,
                    Seconds = value.Seconds,
                    Thousands = value.Thousands
                };


                await EntryCollection.InsertOneAsync(entry);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong creating entry");
            }
        }

        // PUT api/<EntryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EntryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
