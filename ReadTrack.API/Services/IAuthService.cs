using System.Threading.Tasks;
using ReadTrack.Shared.Models;
using ReadTrack.Shared.Models.Requests;

namespace ReadTrack.API.Services;

public interface IAuthService : IService 
{
    Task<TokenResponse> LoginAsync(AuthRequest request);
}