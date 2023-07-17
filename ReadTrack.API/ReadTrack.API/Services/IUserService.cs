using System.Threading.Tasks;
using ReadTrack.API.Models;

namespace ReadTrack.API.Services;

public interface IUserService : IService
{    
    Task<User> GetUserByEmailAsync(string email);
    Task<TokenResponse> CreateUserAsync(CreateUserRequest request);   
}