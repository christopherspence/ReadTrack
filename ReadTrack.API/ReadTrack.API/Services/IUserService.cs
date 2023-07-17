using System.Threading.Tasks;
using ReadTrack.API.Models;

namespace ReadTrack.API.Services;

public interface IUserService
{    
    Task<User> GetUserByEmailAsync(string email);
    Task<User> CreateUserAsync();   
}