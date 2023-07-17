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
    private readonly IAuthService authService;
    public UserService(
        ILogger<UserService> logger, 
        ReadTrackContext context, 
        IAuthService authService,
        IMapper mapper) : base(logger, context, mapper) 
        => this.authService = authService;
    
    public async Task<User> GetUserByEmailAsync(string email)
    {
        var entity = await Context.Users.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);   

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


        return await authService.LoginAsync(new AuthRequest
        {
            Email = request.Email,
            Password = request.Password
        });
    }
}