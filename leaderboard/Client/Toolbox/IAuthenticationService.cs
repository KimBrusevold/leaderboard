using leaderboard.Shared;
using Microsoft.AspNetCore.Components.Authorization;

namespace leaderboard.Client.Toolbox
{
    public interface IAuthenticationService
    {
        Task<LoginResponse?> CheckLogin(string code);
        Task Logut();
        string? GetUserDiscordIdClaim(AuthenticationState state);
        string? GetAvatarIdClaim(AuthenticationState state);
    }
}