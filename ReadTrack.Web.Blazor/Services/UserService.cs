using Blazored.LocalStorage;
using ReadTrack.Shared;
using ReadTrack.Web.Blazor.Api;

namespace ReadTrack.Web.Blazor.Services;

public class UserService : BaseService<UserService>, IUserService
{    
    private readonly IUserApi userApi;
    private ILocalStorageService localStorageService;

    public UserService(ILogger<UserService> logger, IUserApi userApi, ILocalStorageService localStorageService)
        : base(logger)
    {
        this.userApi = userApi;
        this.localStorageService = localStorageService;
    }

    public async Task<bool> IsLoggedInAsync()
        => (await localStorageService.GetItemAsStringAsync(Constants.UserToken)) != null;

    public async Task<string?> GetTokenAsync()
        => await localStorageService.GetItemAsStringAsync(Constants.UserToken);

    public async Task<TokenResponse> RegisterAsync(CreateUserRequest request)
    {
        var response = await userApi.RegisterAsync(request);

        await SetTokenInfoAsync(response);

        return response;
    }

    public async Task<User> GetUserAsync()
        => await userApi.GetUserAsync();

    
    public async Task LogoutAsync()
        => await localStorageService.RemoveItemsAsync([
            Constants.UserToken,
            Constants.UserId,
            Constants.UserEmail,
            Constants.ExpiresAt
        ]);

    

    public async Task SetUserInfoAsync(User user)
    {
        if (user.Email != null)
        {
            await localStorageService.SetItemAsync(Constants.UserId, user.Id);
            await localStorageService.SetItemAsStringAsync(Constants.UserEmail, user.Email);
        }
    }

    public async Task SetTokenInfoAsync(TokenResponse response)
    {
        if (response.Token != null)
        {
            await localStorageService.SetItemAsStringAsync(Constants.UserToken, response.Token);
            await localStorageService.SetItemAsync(Constants.ExpiresAt, response.Expires);
        }        
    }

    public Task UpdateUserAsync(User user)
        => userApi.UpdateUserAsync(user.Id, user);    
}