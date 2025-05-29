using ReadTrack.Shared.Models;
using ReadTrack.Shared.Models.Requests;

namespace ReadTrack.App.MAUI.Services;

public interface IAuthService : IService
{
    Task<TokenResponse> LoginAsync(AuthRequest request);
}
