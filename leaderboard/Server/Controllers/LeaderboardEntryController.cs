using leaderboard.Server.Models;
using leaderboard.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace leaderboard.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardEntryController : ControllerBase
    {
        public LeaderboardContext Context { get; set; }
        public LeaderboardEntryController(LeaderboardContext context)
        {
            Context = context;
        }
        // GET: api/<TrackController>
        [HttpGet]
        public async Task<List<LeaderboardEntry>> Get()
        {
            return await Context.LeaderboardEntries.ToListAsync();
        }

        // GET api/<TrackController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await Context.LeaderboardEntries.Where(t => t.EntryId == id).FirstOrDefaultAsync();
            if (user == null)
                return NotFound();

            return new OkObjectResult(user);
        }

        // POST api/<TrackController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateLeaderboardEntry reqEntry)
        {
            var track = await Context.Tracks.Where(track => track.TrackId == reqEntry.TrackId).FirstOrDefaultAsync();
            var vehicle = await Context.Vehicles.Where(vehicle => vehicle.VehicleId == reqEntry.VehicleId).FirstOrDefaultAsync();
            var user = await Context.Users.Where(user => user.UserId == reqEntry.UserId).FirstOrDefaultAsync();

            

            if (track == null)
                return new NotFoundObjectResult($"No track with Id {reqEntry.TrackId}");
            if (vehicle == null)
                return new NotFoundObjectResult($"No track with Id {reqEntry.VehicleId}");
            if (user == null)
                return new NotFoundObjectResult($"No track with Id {reqEntry.UserId}");

            var entry = new LeaderboardEntry
            {
                Track = track,
                Vehicle = vehicle,
                User = user,
                Time = new TimeSpan(0, 0, 1, 3,23)
            };

            Context.Add(entry);
            await Context.SaveChangesAsync();
            return Created(entry.EntryId.ToString(), entry);
        }

        // PUT api/<TrackController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TrackController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
