using System.Security.Claims;
using System.Threading.Tasks;
using leaderboard.Client.Toolbox;
using Microsoft.AspNetCore.Components.Authorization;

public class AuthState : AuthenticationStateProvider
{
    LocalStorage Storage;
    public AuthState(LocalStorage storage) 
        => Storage = storage;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var cookie = await Storage.GetFromLocalStorage("StateKey");
        
        if(string.IsNullOrWhiteSpace(cookie))
            Console.WriteLine("Cookie does not exists");
        else
            System.Console.WriteLine("Cookie exists");

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "mrfibuli"),
        }, "Fake authentication type");

        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }
}