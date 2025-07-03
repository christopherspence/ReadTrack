using ReadTrack.Shared.Models;
using ReadTrack.Shared.Models.Requests;
using Refit;

namespace ReadTrack.Web.Blazor.Api;

public interface IAuthApi
{
    [Post("/api/auth/login")]
    public Task<TokenResponse> LoginAsync(AuthRequest request);
}