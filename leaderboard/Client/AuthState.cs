using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using leaderboard.Client.Toolbox;
using Microsoft.AspNetCore.Components.Authorization;

namespace leaderboard.Client;
public class AuthState : AuthenticationStateProvider
{
    private readonly LocalStorage Storage;
    private readonly HttpClient HttpClient;
    private readonly AuthenticationState Anonymous = new AuthenticationState(new ClaimsPrincipal());
    private readonly JwtSecurityTokenHandler JwtHandler;
    public AuthState(LocalStorage storage, HttpClient httpClient, JwtSecurityTokenHandler jwtHandler)
    {
        Storage = storage;
        HttpClient = httpClient;
        JwtHandler = jwtHandler;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await Storage.GetItem("authToken");
        Console.WriteLine($"cookie: {token}");


        if (string.IsNullOrWhiteSpace(token) || !JwtHandler.CanReadToken(token))
        {
            Console.WriteLine("Cookie does not exists");
            return Anonymous;
        }

        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);


        
        var identity = new ClaimsIdentity(GetClaims(in token),"jwtAuthType");
        
        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    public void NotifyUserAuthentication(string token)
    {
        var identity = new ClaimsIdentity(GetClaims(in token),"jwtAuthType");

        var user = new ClaimsPrincipal(identity);

        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(Anonymous);
        NotifyAuthenticationStateChanged(authState);
    }

    private IEnumerable<Claim> GetClaims(in string token)
    {
        var jwtToken = JwtHandler.ReadJwtToken(token);

        Console.WriteLine("Claims:");

        foreach (var item in jwtToken.Claims)
        {
            Console.WriteLine($"{item.Type}, {item.Value}");
        }

        return jwtToken.Claims;
    }
}
