using ReadTrack.Shared;
using Refit;

namespace ReadTrack.Web.Blazor.Api;

public interface IUserApi
{
    [Get("/api/user")]
    Task<User> GetUserAsync();

    [Post("/api/user/register")]
    Task<TokenResponse> RegisterAsync(CreateUserRequest request);

    [Put("/api/user/{id}")]
    Task UpdateUserAsync(int userId, User user);
}