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

    public VehicleController(IMongoDatabase mongoDatabase)
    {
        Database = mongoDatabase;
    }


    // GET: api/<VehicleController>
    [HttpGet]
    public async Task<IEnumerable<Vehicle>> Get()
    {
        var vehicleCol = Database.GetCollection<Vehicle>(CollectionNames.VehicleCollection);
        return await (await vehicleCol.FindAsync(FilterBuilder.Empty)).ToListAsync();
    }

    // GET api/<VehicleController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<VehicleController>
    [HttpPost]
    public async Task Post()
    {
        var vehicle = new Vehicle
        {
            Name = "Ferrari 488 GT3 Evo 2020"
        };

        var vehicleCol = Database.GetCollection<Vehicle>(CollectionNames.VehicleCollection);
        await vehicleCol.InsertOneAsync(vehicle);
    }

    // PUT api/<VehicleController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<VehicleController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}

