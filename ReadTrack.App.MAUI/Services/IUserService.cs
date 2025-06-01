using System.Threading.Tasks;
using ReadTrack.Shared.Models;

namespace ReadTrack.App.MAUI.Services;

public interface IUserService : IService
{
    Task<bool> IsLoggedInAsync();
    Task<string?> GetTokenAsync();
    Task SetTokenInfoAsync(TokenResponse response);
    Task SetUserInfoAsync(User user);
    Task LogOutAsync();
}