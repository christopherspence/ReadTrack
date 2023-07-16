using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ReadTrack.API.Data;
using ReadTrack.API.Models;

namespace ReadTrack.API.Services;

public class UserService : BaseService<UserService>, IUserService
{
    public UserService(ILogger<UserService> logger, ReadTrackContext context, IMapper mapper) : base(logger, context, mapper) { }

    public async Task<User> GetUserByCredentialsAsync(string email, string password)
    {
        throw new System.NotImplementedException();
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        throw new System.NotImplementedException();
    }

    public async Task<User> CreateUserAsync()
    {
        throw new System.NotImplementedException();
    }
}