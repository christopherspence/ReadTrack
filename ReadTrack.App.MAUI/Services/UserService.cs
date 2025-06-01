using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReadTrack.App.MAUI.Utilities;
using ReadTrack.Shared.Api;
using ReadTrack.Shared.Models;

namespace ReadTrack.App.MAUI.Services;

public class UserService : BaseService<UserService, IApi>, IUserService
{
    private readonly ILocalStorageService localStorageService;

    public UserService(ILogger<UserService> logger, IAuthApi api, ILocalStorageService localStorageService) : base(logger, api)
        => this.localStorageService = localStorageService;

    public async Task<bool> IsLoggedInAsync()
    {
        return !string.IsNullOrEmpty(await localStorageService.GetAsync(Constants.UserToken));
    }

    public async Task<string?> GetTokenAsync()
        => await localStorageService.GetAsync(Constants.UserToken);

    public async Task LogOutAsync()
    {
        await localStorageService.SetAsync(Constants.UserToken, string.Empty);
        await localStorageService.SetAsync(Constants.ExpiresAt, string.Empty);
    }

    public async Task SetTokenInfoAsync(TokenResponse response)
    {
        if (string.IsNullOrEmpty(response.Token))
        {
            throw new NullReferenceException("token is null or empty");
        }

        await localStorageService.SetAsync(Constants.UserToken, response.Token);
        await localStorageService.SetAsync(Constants.ExpiresAt, response.Expires.ToString());

    }

    public async Task SetUserInfoAsync(User user)
    {
        if (string.IsNullOrEmpty(user.Email))
        {
            throw new NullReferenceException("email should not be empty");
        }

        await localStorageService.SetAsync(Constants.UserId, user.Id.ToString());
        await localStorageService.SetAsync(Constants.UserEmail, user.Email);
    }
}