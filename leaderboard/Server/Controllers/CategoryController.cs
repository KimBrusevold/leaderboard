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
    public class CategoryController : ControllerBase
    {
        private readonly IMongoDatabase Database;
        private static FilterDefinitionBuilder<Category> FilterBuilder = Builders<Category>.Filter;
        private readonly ILogger Logger;
        public CategoryController(IMongoDatabase database, ILogger<CategoryController> logger)
        {
            Database = database;
            Logger = logger;
        }
        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<IEnumerable<Category>> Get(string? name)
        {
            var collection = Database.GetCollection<Category>(CollectionNames.CategoryCollection);
            var allFilter = FilterBuilder.Empty;

            var allCategories = await collection.Find(allFilter).ToListAsync();

            return allCategories;
        }

        
        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CategoryController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category createObject)
        {
            if(string.IsNullOrWhiteSpace(createObject.Name))
                return BadRequest("Need to provide Name");

            var collectionQuery = Database.GetCollection<Category>(CollectionNames.CategoryCollection);

            var findByName = FilterBuilder.Eq(cat => cat.Name, createObject.Name);
            var res = await collectionQuery.Find(findByName).FirstOrDefaultAsync();
            
            if(res is not null)
                return BadRequest($"Category with name {createObject.Name} already exists with Id: {res.Id}");

            
            await collectionQuery.InsertOneAsync(createObject);


            return Ok();
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
