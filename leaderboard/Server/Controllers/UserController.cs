using leaderboard.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace leaderboard.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public LeaderboardContext Context { get; set; }
        public UserController(LeaderboardContext context)
        {
            Context = context;
        }
        // GET: api/<TrackController>
        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await Context.Users.ToListAsync();
        }

        // GET api/<TrackController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await Context.Users.Where(t => t.UserId== id).FirstOrDefaultAsync();
            if (user == null)
                return NotFound();

            return new OkObjectResult(user);
        }

        // POST api/<TrackController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User reqUser)
        {
            if (string.IsNullOrWhiteSpace(reqUser.Name))
                return new BadRequestObjectResult("Creation failed. Name is Requiered");

            var user = new User
            {
                Name = reqUser.Name,
                Email = reqUser.Email
            };

            Context.Add(user);
            await Context.SaveChangesAsync();
            return Created(user.UserId.ToString(), user);
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
