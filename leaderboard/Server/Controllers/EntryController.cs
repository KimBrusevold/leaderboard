using leaderboard.Shared;
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

        public EntryController(IMongoDatabase mongoDatabase)
        {
            Database = mongoDatabase;
        }


        // GET: api/<EntryController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //Database.GetCollection<>(CollectionNames.UserCollection);

            return new string[] { "value1", "value2" };
        }

        // GET api/<EntryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<EntryController>
        [HttpPost]
        public void Post([FromBody] Entry value)
        {
            var entryCollection = Database.GetCollection<Entry>(CollectionNames.EntryCollection);

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
