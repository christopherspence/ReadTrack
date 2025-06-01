using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReadTrack.Shared.Api;
using ReadTrack.Shared.Models;
using ReadTrack.Shared.Models.Requests;

namespace ReadTrack.App.MAUI.Services;

public class AuthService : BaseService<AuthService, IAuthApi>, IAuthService
{
    private readonly IUserService userService;

    public AuthService(ILogger<AuthService> logger, IAuthApi api, IUserService userService) : base(logger, api)
        => this.userService = userService;

    public async Task<TokenResponse> LoginAsync(AuthRequest request)
    {
        var response = await Api.LoginAsync(request);

        await userService.SetTokenInfoAsync(response);
        await userService.SetUserInfoAsync(response.User!);

        return response;
    }
}