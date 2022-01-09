using leaderboard.Shared;

namespace leaderboard.Client.Toolbox
{
    public interface IAuthenticationService
    {
        Task<LoginResponse?> CheckLogin(string code);
        Task Logut();
    }
}