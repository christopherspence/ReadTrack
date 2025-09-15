using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReadTrack.API.Data;
using ReadTrack.API.Data.Entities;
using ReadTrack.Shared;

namespace ReadTrack.API.Services;

public class UserService : BaseService<UserService>, IUserService
{
    private readonly IPasswordHasher<User> hasher;
    private readonly ITokenService tokenService;
    public UserService(
        ILogger<UserService> logger, 
        ReadTrackContext context, 
        IPasswordHasher<User> hasher,
        ITokenService tokenService,
        IMapper mapper) : base(logger, context, mapper) 
    {
        this.tokenService = tokenService;
        this.hasher = hasher;
    }
    
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var entity = await Context.Users.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);   

        if (entity == null)
        {
            return null;
        }

        return Mapper.Map<UserEntity, User>(entity);
    }

    public async Task<User?> GetUserAsync(int id, bool includePassword = false)
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

    public async Task<TokenResponse?> CreateUserAsync(CreateUserRequest request)
    {
        if (await Context.Users.AnyAsync(u => u.Email == request.Email && !u.IsDeleted))
        {
            throw new ApplicationException($"Email \"{request.Email}\" already exists.");    
        }

        var entity = new UserEntity
        {
            Email = request.Email ?? string.Empty,
            Password = request.Password ?? string.Empty,
            FirstName = request.FirstName ?? string.Empty,
            LastName = request.LastName ?? string.Empty,
            ProfilePicture = string.Empty,
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow
        };

        // Hash the user's password
        var user = Mapper.Map<UserEntity, User>(entity);
        entity.Password = hasher.HashPassword(user, request.Password ?? string.Empty);

        await Context.Users.AddAsync(entity);
        await Context.SaveChangesAsync();


        return tokenService.GenerateToken(user);
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

        existingEntity.FirstName = user.FirstName ?? string.Empty;
        existingEntity.LastName = user.LastName ?? string.Empty;
        existingEntity.Email = user.Email ?? string.Empty;        
        existingEntity.ProfilePicture = user.ProfilePicture ?? string.Empty;

        // only update the password if they specified a new one and it doensn't match the old one
        if (!string.IsNullOrEmpty(user.Password))
        {
            // have to use the existing entity, otherwise the hasher won't has properly
            var passwordHash = hasher.HashPassword(Mapper.Map<UserEntity, User>(existingEntity), user.Password);
            if (existingEntity.Password != passwordHash) 
            {
                existingEntity.Password = passwordHash;    
            }            
        }
        
        Context.Entry(existingEntity).State = EntityState.Modified;
        await Context.SaveChangesAsync();

        return true;
    }
}