using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReadTrack.API.Data;
using ReadTrack.API.Data.Entities;
using ReadTrack.API.Models;

namespace ReadTrack.API.Services;

public class UserService : BaseService<UserService>, IUserService
{
    public UserService(ILogger<UserService> logger, ReadTrackContext context, IMapper mapper) : base(logger, context, mapper) { }
    
    public async Task<User> GetUserByEmailAsync(string email)
    {
        var entity = await Context.Users.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);   

        if (entity == null)
        {
            return null;
        }

        return Mapper.Map<UserEntity, User>(entity);
    }

    public async Task<User> CreateUserAsync()
    {
        throw new System.NotImplementedException();
    }
}