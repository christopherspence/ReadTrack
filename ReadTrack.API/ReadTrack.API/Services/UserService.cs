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

    public async Task<User> GetUserAsync(int id, bool includePassword = false)
    {
        var entity = await Context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);   

        if (entity == null)
        {
            return null;
        }

        if (includePassword == false)
        {
            entity.Password = string.Empty;
        }

        return Mapper.Map<UserEntity, User>(entity);
    }

    public async Task<TokenResponse> CreateUserAsync(CreateUserRequest request)
    {
        if (await Context.Users.AnyAsync(u => u.Email == request.Email && !u.IsDeleted))
        {
            throw new ApplicationException($"Email \"{request.Email}\" already exists.");    
        }

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

    public async Task<bool> UpdateUserAsync(int userId, User user)
    {
        // Make sure the user that's being updated belongs to the user id defined in the token
        if (userId != user.Id)
        {
            return false;
        }

        var existingEntity = await Context.Users.SingleOrDefaultAsync(u => u.Id == user.Id && !u.IsDeleted);

        if (existingEntity == null)
        {
            return false;
        }

        existingEntity.FirstName = user.FirstName;
        existingEntity.LastName = user.LastName;
        existingEntity.Email = user.Email;        
        existingEntity.ProfilePicture = user.ProfilePicture;
        
        Context.Entry(existingEntity).State = EntityState.Modified;
        await Context.SaveChangesAsync();

        return true;
    }
}