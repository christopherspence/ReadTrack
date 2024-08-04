using System.Threading.Tasks;
using ReadTrack.Shared;
using ReadTrack.Shared.Requests;

namespace ReadTrack.API.Services;

public interface IAuthService : IService 
{
    Task<TokenResponse> LoginAsync(AuthRequest request);
}