using ReadTrack.Shared;

namespace ReadTrack.Web.Blazor.Services;

public interface IUserService : IService
{
    Task<bool> IsLoggedInAsync();

    Task<User> GetUserAsync();

    Task<TokenResponse> RegisterAsync(CreateUserRequest request);

    Task UpdateUserAsync(User user);

    Task<string?> GetTokenAsync();    

    Task SetUserInfoAsync(User user);

    Task LogoutAsync();
}