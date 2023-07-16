using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ReadTrack.API.Data;
using ReadTrack.API.Models;
using ReadTrack.API.Models.Requests;

namespace ReadTrack.API.Services;

public class AuthService : BaseService<AuthService>, IAuthService
{
    private readonly ITokenService tokenService;
    private readonly IUserService userService; 

    public AuthService(
        ILogger<AuthService> logger, 
        ReadTrackContext context, 
        ITokenService tokenService,
        IUserService userService,
        IMapper mapper) : base(logger, context, mapper)
    {
        this.tokenService = tokenService;
        this.userService = userService;
    }

    public async Task<TokenResponse> LoginAsync(AuthRequest request)
    {
        throw new System.NotImplementedException();
    }
}