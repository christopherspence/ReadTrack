using ReadTrack.Shared;
using ReadTrack.Shared.Requests;
using ReadTrack.Web.Blazor.Api;

namespace ReadTrack.Web.Blazor.Services;

public class AuthService : BaseService<AuthService>, IAuthService
{
    private readonly IAuthApi authApi;
    private readonly IUserService userService;

    public AuthService(ILogger<AuthService> logger, IAuthApi authApi, IUserService userService) : base(logger)
    {
        this.authApi = authApi;
        this.userService = userService;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var response = await authApi.LoginAsync(new AuthRequest
        {
            Email = email,
            Password = password
        });

        if (response.User == null)
        {
            Logger.LogError("User missing in server response");
            return false;
        }

        await userService.SetTokenInfoAsync(response);
        await userService.SetUserInfoAsync(response.User);

        return true;
    }
}
