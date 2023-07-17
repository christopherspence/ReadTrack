using System.IdentityModel.Tokens.Jwt;
using ReadTrack.API.Models;

namespace ReadTrack.API.Services;

public interface ITokenService : IService
{
    TokenResponse GenerateToken(User user);
    JwtSecurityToken DecodeToken(string token);
}