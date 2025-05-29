using Microsoft.Extensions.Logging;
using ReadTrack.Shared.Api;
using ReadTrack.Shared.Models;

namespace ReadTrack.App.MAUI.Services;

public class UserService : BaseService<UserService, IApi>, IUserService
{
    const string UserToken = "user_token";
    const string ExpiresAt = "expires_at";
    const string UserId = "user_id";
    const string UserEmail = "user_email";

    private readonly ILocalStorageService localStorageService;

    public UserService(ILogger<UserService> logger, IAuthApi api, ILocalStorageService localStorageService) : base(logger, api)
        => this.localStorageService = localStorageService;

    public async Task<bool> IsLoggedInAsync()
    {
        return await localStorageService.GetAsync(UserToken) != null;
    }

    public async Task SetTokenInfoAsync(TokenResponse response)
    {
        if (string.IsNullOrEmpty(response.Token))
        {
            throw new NullReferenceException("token is null or empty");
        }

        await localStorageService.SetAsync(UserToken, response.Token);
        await localStorageService.SetAsync(ExpiresAt, response.Expires.ToString());

    }

    public async Task SetUserInfoAsync(User user)
    {
        if (string.IsNullOrEmpty(user.Email))
        {
            throw new NullReferenceException("email should not be empty");
        }

        await localStorageService.SetAsync(UserId, user.Id.ToString());
        await localStorageService.SetAsync(UserEmail, user.Email);
    }
}