using ReadTrack.Shared;
using ReadTrack.Shared.Requests;
using Refit;

namespace ReadTrack.Web.Blazor.Api;

public interface IAuthApi
{
    [Post("/api/auth/login")]
    public TokenResponse LoginAsync(AuthRequest request);
}