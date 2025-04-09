using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ReadTrack.API.Data;
using ReadTrack.Shared;
using ReadTrack.Shared.Requests;

namespace ReadTrack.API.Services;

public class AuthService : BaseService<AuthService>, IAuthService
{
    private readonly IPasswordHasher<User> hasher;
    private readonly ITokenService tokenService;
    private readonly IUserService userService; 

    public AuthService(
        ILogger<AuthService> logger, 
        ReadTrackContext context, 
        IPasswordHasher<User> hasher,
        ITokenService tokenService,
        IUserService userService,
        IMapper mapper) : base(logger, context, mapper)
    {
        this.hasher = hasher;
        this.tokenService = tokenService;
        this.userService = userService;
    }

    public async Task<TokenResponse> LoginAsync(AuthRequest request)
    {
        var user = await userService.GetUserByEmailAsync(request.Email);

        if (user == null)
        {
            return null;
        }

        var result = hasher.VerifyHashedPassword(user, user.Password, request.Password);   

        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }     

        return tokenService.GenerateToken(user);
    }
}