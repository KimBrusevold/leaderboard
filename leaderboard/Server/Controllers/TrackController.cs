using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace leaderboard.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        public LeaderboardContext _Context { get; set; }
        public TrackController(LeaderboardContext context)
        {
            _Context = context;
        }
        // GET: api/<TrackController>
        [HttpGet]
        public async Task<List<Track>> Get()
        {
            return await _Context.Tracks.ToListAsync();
        }

        // GET api/<TrackController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var track = await _Context.Tracks.Where(t => t.TrackId == id).FirstOrDefaultAsync();
            if (track == null)
                return NotFound();

            return new OkObjectResult(track);
        }

        // POST api/<TrackController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Shared.Track track)
        {
            if (string.IsNullOrWhiteSpace(track.Name))
                return new BadRequestObjectResult("Creation failed. Name is Requiered");

            var createTrack = new Track()
            {
                Name = track.Name,
            };

            _Context.Add(createTrack);
            await _Context.SaveChangesAsync();
            return Created(createTrack.TrackId.ToString(), createTrack);
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
