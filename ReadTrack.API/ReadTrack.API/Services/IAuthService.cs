using System.Threading.Tasks;
using ReadTrack.API.Models;
using ReadTrack.API.Models.Requests;

namespace ReadTrack.API.Services;

public interface IAuthService : IService 
{
    Task<TokenResponse> LoginAsync(AuthRequest request);
}