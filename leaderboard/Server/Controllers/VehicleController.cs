using leaderboard.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace leaderboard.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private LeaderboardContext Context { get; set; }

        public VehicleController(LeaderboardContext context)
        {
            Context = context;
        }


        // GET: api/<VehicleController>
        [HttpGet]
        public async Task<List<Vehicle>> Get()
        {
            return await Context.Vehicles.ToListAsync();
        }

        // GET api/<VehicleController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> Get(int id)
        {
            var res = await Context.Vehicles.Where(v => v.VehicleId == id).FirstOrDefaultAsync();
            if (res == null)
                return NotFound();
            return new OkObjectResult(res);
        }

        // POST api/<VehicleController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Vehicle vehicle)
        {
            if (string.IsNullOrWhiteSpace(vehicle.Name))
                return new BadRequestObjectResult("Creation failed. Name is Requiered");

            var createVehicle = new Vehicle()
            {
                Name = vehicle.Name,
            };

            Context.Add(createVehicle);
            await Context.SaveChangesAsync();
            return Created(createVehicle.VehicleId.ToString(), createVehicle);
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
}
