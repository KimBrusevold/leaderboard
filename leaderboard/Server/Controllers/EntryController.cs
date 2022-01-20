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
        IMongoCollection<EntryCreateobj> EntryCollection;
        private static FilterDefinitionBuilder<EntryCreateobj> FilterBuilder = Builders<EntryCreateobj>.Filter;


        public EntryController(IMongoDatabase mongoDatabase)
        {
            Database = mongoDatabase;
            EntryCollection = Database.GetCollection<EntryCreateobj>(CollectionNames.EntryCollection);
        }


        // GET: api/<EntryController>
        [HttpGet]
        public async Task <IEnumerable<Shared.RetrieveObjects.Entry>> Get(string gameId, string? trackId)
        {
            // var filter = FilterBuilder.Eq(x => x.Game.Id, gameId);
            FilterDefinition<EntryCreateobj>[] filters = new FilterDefinition<EntryCreateobj>[2];
            filters[0] = FilterBuilder.Eq(ent => ent.Game.Id, gameId);
            
            
            if(!string.IsNullOrWhiteSpace(trackId))
            {
                filters[1] = FilterBuilder.Eq(ent => ent.Track.Id, trackId);
            }
            var filter = FilterBuilder.And(
                filters
            );

            var entries = await EntryCollection.Find(filter)
                .SortBy(e => e.Time)
                .ToListAsync();
            var userGroup = entries.GroupBy(ent => ent.User.Id);

            var bestEntries = new List<Shared.RetrieveObjects.Entry>(userGroup.Count());
            
            var rank = 1;
            foreach (var ent in userGroup)
            {
                var entry = ent.First();
                entry.Rank = rank;
                rank++;
                bestEntries.Add(new Shared.RetrieveObjects.Entry(){
                    Game = entry.Game,
                    Rank = entry.Rank,
                    Time = TimeSpan.FromSeconds(entry.Time),
                    User = entry.User,
                    Vehicle = entry.Vehicle,
                    Track = entry.Track
                });
            }

        

            return bestEntries;
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
                EntryCreateobj entry = new() {
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
                    Time = value.Time.TotalSeconds
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
