using leaderboard.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using IO = System.IO;
namespace leaderboard.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly HttpClient DiscordClient = new();
        private readonly Uri MainDiscordApiUri;
        private readonly IMongoDatabase Database;
        private readonly ILogger Logger;
        public LoginController(IConfiguration configuration, IMongoDatabase mongoDatabase, ILogger<LoginController> logger)
        {
            Configuration = configuration;
            MainDiscordApiUri = new Uri((string)Configuration["Discord:url"]);
            Database = mongoDatabase;
            Logger = logger;
        }

        [Authorize]
        [HttpGet]
        public ActionResult IsLoggedIn()
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginData data)
        {
            var formData = new Dictionary<string, string>
            {
                {"client_id", Configuration["Discord:clientId"] },
                {"client_secret", Configuration["Discord:clientSecret"] },
                {"grant_type", "authorization_code" },
                {"code", data.Code },
                {"redirect_uri", Configuration["Discord:redirectUri"] },
            };

            var form = new FormUrlEncodedContent(formData);

            var req = new HttpRequestMessage(HttpMethod.Post, $"{MainDiscordApiUri}/oauth2/token")
            {
                Content = form
            };

            var res = await DiscordClient.SendAsync(req);

            if (!res.IsSuccessStatusCode)
                return Unauthorized(new {Reponse = "Could not authenticate against Discord \n" +await res.Content.ReadAsStringAsync()});

            Logger.LogInformation("User is Authneticated");
            //Add check to see if user is logged in from before, then no need to get data from Discord

            var authResponse =await res.Content.ReadFromJsonAsync<DiscordAuthResponse>();

            var userData = await RetrieveUserDiscordData(authResponse);
            
            if(userData == null)
                return new BadRequestObjectResult(new {Reponse = "Could not retrieve information from Discord, is user authenticated?"});


            if (await UserExists(userData.Value) is false)
                await CreateUser(userData.Value);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userData.Value.Username),
                new Claim("DiscordId", userData.Value.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,"User"),
                new Claim("DiscordAvatarId", userData.Value.Avatar)
            };



            var secretKey = Configuration["JWT:key"];
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var token = new JwtSecurityToken(
                issuer: Configuration["JWT:iss"],
                audience: Configuration["JWT:aud"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token), Expires = DateTime.Now.AddHours(3) });

        }

        private async Task CreateUser(DiscordMe userData)
        {
            var userCol = Database.GetCollection<Shared.RetrieveObjects.User>(CollectionNames.UserCollection);

            var user = new Shared.RetrieveObjects.User
            {
                DiscordId = userData.Id,
                UserName = userData.Username,
                ImageId = userData.Avatar,
            };

            await userCol.InsertOneAsync(user);
        }

        private async Task<bool> UserExists(DiscordMe userData)
        {
            var userCol = Database.GetCollection<Shared.RetrieveObjects.User>(CollectionNames.UserCollection);

            var filter = Builders<Shared.RetrieveObjects.User>.Filter.Eq("DiscordId", userData.Id);

            var res = await userCol.FindAsync(filter);
            return res.Any();


        }

        private async Task<byte[]> RetrieveAndSaveUserImage(DiscordMe userData)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"https://cdn.discordapp.com/avatars/{userData.Id}/{userData.Avatar}.png");
            var res = await DiscordClient.SendAsync(req);

            if (!res.IsSuccessStatusCode)
                throw new NotImplementedException();

            var userImage = await res.Content.ReadAsByteArrayAsync();
            return userImage;
        }


        private async Task<DiscordMe?> RetrieveUserDiscordData(DiscordAuthResponse authResponse)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"{MainDiscordApiUri}/users/@me");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.access_token);

            var res = await DiscordClient.SendAsync(req);

            if (!res.IsSuccessStatusCode)
                return null;

            var discordMe = await res.Content.ReadFromJsonAsync<DiscordMe>();

            return discordMe;
        }

        public readonly struct DiscordAuthResponse
        {
            public string access_token { get; init; }
            public string token_type { get; init; }
            public int expires_in { get; init; }
            public string refresh_token { get; init; }
            public string scope { get; init; }

            public DiscordAuthResponse(string access_token, string token_type, int expires_in, string refresh_token, string scope)
            {
                this.access_token = access_token;
                this.token_type = token_type;
                this.expires_in = expires_in;
                this.refresh_token = refresh_token;
                this.scope = scope;
            }
        }

        public readonly struct DiscordMe
        {
            public string Id { get; init; }
            public string Username { get; init; }
            public string Discriminator { get; init; }
            public string Avatar { get; init; }

        }
    }
}
