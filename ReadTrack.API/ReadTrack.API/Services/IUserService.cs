using System.Threading.Tasks;
using ReadTrack.Shared;

namespace ReadTrack.API.Services;

public interface IUserService : IService
{    
    Task<User?> GetUserAsync(int id, bool includePassword = false);
    Task<User?> GetUserByEmailAsync(string email);
    Task<TokenResponse?> CreateUserAsync(CreateUserRequest request);  
    Task<bool> UpdateUserAsync(int userId, User user); 
}