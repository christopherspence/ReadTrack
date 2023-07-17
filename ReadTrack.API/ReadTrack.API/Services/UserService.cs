using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReadTrack.API.Data;
using ReadTrack.API.Data.Entities;
using ReadTrack.API.Models;
using ReadTrack.API.Models.Requests;

namespace ReadTrack.API.Services;

public class UserService : BaseService<UserService>, IUserService
{
    private readonly ITokenService tokenService;
    public UserService(
        ILogger<UserService> logger, 
        ReadTrackContext context, 
        ITokenService tokenService,
        IMapper mapper) : base(logger, context, mapper) 
        => this.tokenService = tokenService;
    
    public async Task<User> GetUserByEmailAsync(string email)
    {
        var entity = await Context.Users.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);   

        if (entity == null)
        {
            return null;
        }

        return Mapper.Map<UserEntity, User>(entity);
    }

    public async Task<User> GetUserAsync(int id)
    {
        var entity = await Context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);   

        if (entity == null)
        {
            return null;
        }

        return Mapper.Map<UserEntity, User>(entity);
    }

    public async Task<TokenResponse> CreateUserAsync(CreateUserRequest request)
    {
        var entity = new UserEntity
        {
            Email = request.Email,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow
        };

        await Context.Users.AddAsync(entity);
        await Context.SaveChangesAsync();


        return tokenService.GenerateToken(Mapper.Map<UserEntity, User>(entity));
    }
}