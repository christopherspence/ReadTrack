using System.IdentityModel.Tokens.Jwt;
using ReadTrack.Shared;

namespace ReadTrack.API.Services;

public interface ITokenService : IService
{
    TokenResponse GenerateToken(User user);
    JwtSecurityToken DecodeToken(string token);
}