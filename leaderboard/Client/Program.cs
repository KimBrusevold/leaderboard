using leaderboard.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Toolbox = leaderboard.Client.Toolbox;
using leaderboard.Client.Toolbox;
using System.IdentityModel.Tokens.Jwt;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<Toolbox.LocalStorage>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthState>();
builder.Services.AddScoped<JwtSecurityTokenHandler>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMudServices();
builder.Services.AddOptions();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();

