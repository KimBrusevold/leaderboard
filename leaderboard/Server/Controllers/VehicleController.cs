using System.Text.Json;
using Ganss.XSS;
using leaderboard.Shared;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace leaderboard.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleController : ControllerBase
{
    private static FilterDefinitionBuilder<Vehicle> FilterBuilder = Builders<Vehicle>.Filter;
    private readonly IMongoDatabase Database;
    private ILogger<VehicleController> Logger {get; set;}
    private HtmlSanitizer Sanitizer = new HtmlSanitizer();
    public VehicleController(IMongoDatabase mongoDatabase, ILogger<VehicleController> logger)
    {
        Database = mongoDatabase;
        Logger = logger;
    }


    // GET: api/<VehicleController>
    [HttpGet]
    public async Task<IEnumerable<Vehicle>> Get(string? gameId)
    {
        // var vehicles = await (await vehicleCol.FindAsync(FilterBuilder.Empty)).ToListAsync();
        
        if(string.IsNullOrWhiteSpace(gameId) is false)
        {
            var gameCollection = Database.GetCollection<Game>(CollectionNames.GameCollection);
            return await gameCollection.Find(ga => ga.Id == gameId).Project(ga => ga.Vehicles).FirstOrDefaultAsync();

        }
        else
        {
            var collection = Database.GetCollection<Vehicle>(CollectionNames.VehicleCollection);
            return await collection.Find(new BsonDocument()).ToListAsync();
        }

    }

    // GET api/<VehicleController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<VehicleController>
    // [HttpPost]
    // public async Task<IActionResult>Post([FromBody] Vehicle vehicle)
    // {
    //     vehicle.Id = null;
    //     vehicle.Name = Sanitizer.Sanitize(vehicle.Name);
    //     var vehicleCol = Database.GetCollection<Vehicle>(CollectionNames.VehicleCollection);
    //     Logger.LogInformation(JsonSerializer.Serialize(vehicle));
    //     await vehicleCol.InsertOneAsync(vehicle);
    //     return Ok("Went ok");
    // }

    // PUT api/<VehicleController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id)
    {
        var collection = Database.GetCollection<Vehicle>(CollectionNames.VehicleCollection);

        var allVehicles = await collection.Find(FilterBuilder.Empty).ToListAsync();

        var gt3Vehicles = allVehicles.Where(ve => ve.Name.Contains("GT3"));
        System.Console.WriteLine("Count: " + gt3Vehicles.Count());

        var catCol = Database.GetCollection<Category>(CollectionNames.CategoryCollection);
        var category = await catCol.Find(Builders<Category>.Filter.Empty).FirstOrDefaultAsync();
        
        if(category is null)
            return BadRequest("No category Found");

        var updateBuilder = Builders<Vehicle>.Update.Set(ve => ve.Category, category);

        foreach (var item in gt3Vehicles)
        {
            System.Console.WriteLine(item.Name);
            item.Category = category;
            await collection.UpdateOneAsync(FilterBuilder.Eq(ve => ve.Id, item.Id), updateBuilder);
        }

        return Ok();

    }

    // DELETE api/<VehicleController>/5
    // [HttpDelete]
    // public void Delete()
    // {
    //      var trackCol = Database.GetCollection<Vehicle>(CollectionNames.VehicleCollection);
    //         trackCol.DeleteMany(Builders<Vehicle>.Filter.Empty);
    // }
}

