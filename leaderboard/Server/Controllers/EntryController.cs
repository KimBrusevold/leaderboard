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
        public async Task <IEnumerable<Shared.RetrieveObjects.Entry>> Get(string gameId, string? trackId, string? categoryId)
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
            
            foreach (var ent in userGroup)
            {
                var usersBestEntries = new List<EntryCreateobj>();
                
                var bestPerVehicle = ent.GroupBy(gr => gr.Vehicle.Id);


                foreach (var vehicleGroup in bestPerVehicle)
                {
                    var entry = vehicleGroup.First();
                    if(string.IsNullOrWhiteSpace(categoryId) is false)
                    {
                        System.Console.WriteLine("CategoryId to search for is " + categoryId);
                        if(entry.Vehicle.Category?.Id == categoryId)
                        {
                            bestEntries.Add(new Shared.RetrieveObjects.Entry(){
                                                    Game = entry.Game,
                                                    Rank = entry.Rank,
                                                    Time = TimeSpan.FromSeconds(entry.Time),
                                                    User = entry.User,
                                                    Vehicle = entry.Vehicle,
                                                    Track = entry.Track
                                                });
                        }

                        continue;
                    }

                    bestEntries.Add(new Shared.RetrieveObjects.Entry(){
                        Game = entry.Game,
                        Rank = entry.Rank,
                        Time = TimeSpan.FromSeconds(entry.Time),
                        User = entry.User,
                        Vehicle = entry.Vehicle,
                        Track = entry.Track
                    });
                }               
                
            }
            var timeSorted = bestEntries.OrderBy(x => x.Time);
            for(int i = 0; i < timeSorted.Count(); i++)
            {
                timeSorted.ElementAt(i).Rank = i +1;
            }
             
            return timeSorted;
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
        [HttpPut]
        public async Task Put()
        {
            var entryCollection = Database.GetCollection<EntryCreateobj>(CollectionNames.EntryCollection);
            var allEntries = await entryCollection.Find(FilterBuilder.Empty).ToListAsync();

            var vehicleCollectino = Database.GetCollection<Vehicle>(CollectionNames.VehicleCollection);
            
            foreach (var item in allEntries)
            {
                var vehicle = await  vehicleCollectino.Find(Builders<Vehicle>.Filter.Eq(ve => ve.Id, item.Vehicle.Id)).FirstOrDefaultAsync();

                if(vehicle is null)
                    continue;

                var update = Builders<EntryCreateobj>.Update.Set(entry => entry.Vehicle, vehicle);

                await entryCollection.UpdateOneAsync(FilterBuilder.Eq(entry=> entry.Id, item.Id), update);
            }
        }

        // DELETE api/<EntryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
