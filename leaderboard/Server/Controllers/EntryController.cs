﻿using leaderboard.DataProvider.Interfaces;
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
        ILeaderboardDataProvider DataProvider;
        private static FilterDefinitionBuilder<EntryCreateobj> FilterBuilder = Builders<EntryCreateobj>.Filter;


        public EntryController(IMongoDatabase mongoDatabase, ILeaderboardDataProvider dataProvider)
        {
            Database = mongoDatabase;
            EntryCollection = Database.GetCollection<EntryCreateobj>(CollectionNames.EntryCollection);
            DataProvider = dataProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string? discordId, string? gameId, string? trackId, string? categoryId)
        {
            List<Entry>? entries = null;

            var hasGame = string.IsNullOrWhiteSpace(gameId) is false;
            var hasTrack = string.IsNullOrWhiteSpace(trackId) is false;
            var hasCategory = string.IsNullOrWhiteSpace(categoryId) is false;
            
            string? userId = null;

            if(string.IsNullOrWhiteSpace(discordId))
            {
                var user = await DataProvider.Users().GetByDiscrodId(discordId);
                userId = user?.Id;
            }


            try
            {
                if (hasGame && hasTrack is false && hasCategory is false)
                    entries = await DataProvider.Entries().Filter(gameId, userId);
                else if(hasGame && hasTrack && hasCategory is false)
                    entries = await DataProvider.Entries().Filter(gameId, trackId, userId);
                else if(hasGame && hasTrack && hasCategory)
                    entries = await DataProvider.Entries().Filter(gameId, trackId, categoryId, userId);
                else
                    entries =  await DataProvider.Entries().All(userId);

                return Ok(entries);
            }
            catch (Exception e)
            {
                Console.WriteLine("EntryController.Get(): {0}", e);
                return BadRequest("Something went wrong processing request. Contact page admin");
            }
        }
        

        // POST api/<EntryController>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Shared.Entry value)
        {
            var userCollection = Database.GetCollection<Shared.RetrieveObjects.User>(CollectionNames.UserCollection);

            var userBuilder = Builders<Shared.RetrieveObjects.User>.Filter;

            var filter = userBuilder.Eq("DiscordId", value.User.DiscordId);
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
                    Time = value.Time,
                    Created = DateTime.UtcNow
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

        private void FilterByVehicleCategory(ref IEnumerable<Shared.RetrieveObjects.Entry> bestEntries, string categoryId)
        {
            bestEntries = bestEntries.Where(entry => entry.Vehicle.Category?.Id == categoryId);            
        }

        private IEnumerable<Shared.RetrieveObjects.Entry> BestEntryByTime(IEnumerable<IGrouping<string, EntryCreateobj>> userGroup)
        {
            List<Shared.RetrieveObjects.Entry> bestEntries = new();

            foreach (var ent in userGroup)
            {
                var usersBestEntries = new List<EntryCreateobj>();
                
                var bestPerVehicle = ent.GroupBy(gr => gr.Vehicle.Id);

                foreach (var vehicleGroup in bestPerVehicle)
                {
                    var entry = vehicleGroup.First();

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
             
            return bestEntries;
        }
    }
}
