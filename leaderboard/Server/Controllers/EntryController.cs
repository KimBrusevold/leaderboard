﻿using leaderboard.Shared;
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


        public EntryController(IMongoDatabase mongoDatabase)
        {
            Database = mongoDatabase;
            EntryCollection = Database.GetCollection<Shared.RetrieveObjects.Entry>(CollectionNames.EntryCollection);
        }


        // GET: api/<EntryController>
        [HttpGet]
        public async Task <IEnumerable<Shared.RetrieveObjects.Entry>> Get()
        {
            var entries = await EntryCollection.Find(FilterDefinition<Shared.RetrieveObjects.Entry>.Empty)
                .SortBy(e => e.Minutes).ThenBy(e => e.Seconds).ThenBy(e => e.Thousands)
                .ToListAsync();
            
            var rank = 1;
            foreach (var e in entries)
            {
                e.Rank = rank;
                rank++;
            }

            return entries;
        }

        // GET api/<EntryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<EntryController>
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
