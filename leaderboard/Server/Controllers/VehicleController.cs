using System.Text.Json;
using Ganss.XSS;
using leaderboard.Shared;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IEnumerable<Vehicle>> Get()
    {
        var vehicleCol = Database.GetCollection<Vehicle>(CollectionNames.VehicleCollection);
        var vehicles = await (await vehicleCol.FindAsync(FilterBuilder.Empty)).ToListAsync();
        
        foreach (var item in vehicles)
        {
            System.Console.WriteLine(item.Name);
        }

        return vehicles;
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
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<VehicleController>/5
    // [HttpDelete]
    // public void Delete()
    // {
    //      var trackCol = Database.GetCollection<Vehicle>(CollectionNames.VehicleCollection);
    //         trackCol.DeleteMany(Builders<Vehicle>.Filter.Empty);
    // }
}

