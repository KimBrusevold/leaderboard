using leaderboard.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace leaderboard.Client.Toolbox;
public class AuthenticationService : IAuthenticationService
{
    private readonly AuthenticationStateProvider AuthStateProvider;
    private readonly HttpClient Client;
    private readonly LocalStorage Storage;

    public AuthenticationService(AuthenticationStateProvider authStateProvider, HttpClient client, LocalStorage storage)
    {
        AuthStateProvider = authStateProvider;
        Client = client;
        Storage = storage;
    }

    public async Task<LoginResponse?> CheckLogin(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return null;

        Console.WriteLine($"Code is {code}");
        var loginData = new LoginData(code);

        //var auth = await Http.GetFromJsonAsync<LoginResponse>("Login");
        var authResult = await Client.PostAsJsonAsync("/api/Login", loginData);

        if (authResult.IsSuccessStatusCode == false)
            return null;

        var response = await authResult.Content.ReadFromJsonAsync<LoginResponse>();
        Console.WriteLine($"AuthCode is: {response.Token}");

        await Storage.SetItem("authToken", response.Token);

        ((AuthState)AuthStateProvider).NotifyUserAuthentication(response.Token);

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", response.Token);
        return response;
    }

    public async Task Logut()
    {
        await Storage.RemoveItem("authToken");
        Client.DefaultRequestHeaders.Authorization = null;

        ((AuthState)AuthStateProvider).NotifyUserLogout();

    }
}

