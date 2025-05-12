using System.Net.Http.Headers;
using ReadTrack.Web.Blazor.Services;

namespace ReadTrack.Web.Blazor;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly IUserService userService;
    public AuthHeaderHandler(IUserService userService)
        => this.userService = userService;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await userService.GetTokenAsync();
        
        if (token != null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}