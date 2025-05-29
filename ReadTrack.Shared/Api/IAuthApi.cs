using System.Threading.Tasks;
using ReadTrack.Shared.Models;
using ReadTrack.Shared.Models.Requests;
using Refit;

namespace ReadTrack.Shared.Api;

public interface IAuthApi : IApi
{
    [Post("/api/auth/login")]
    Task<TokenResponse> LoginAsync(AuthRequest request);
}