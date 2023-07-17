using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ReadTrack.API.Data;
using ReadTrack.API.Extensions;
using ReadTrack.API.Models;

namespace ReadTrack.API.Services;

public class TokenService : BaseService<TokenService>, ITokenService
{
    private readonly JwtSettings settings;

    public TokenService(ILogger<TokenService> logger, ReadTrackContext context, JwtSettings settings, IMapper mapper) : base(logger, context, mapper)
     => this.settings = settings;

    public TokenResponse GenerateToken(User user)
    {
        var handler = new JwtSecurityTokenHandler();

        if (user == null)
        {
            return null;
        }

        var key = settings.SecretKey.Decode();

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}" ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, "User")
            }),
            Expires = DateTime.UtcNow.AddDays(settings.ExpirationDays),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = settings.Issuer,
            Audience = settings.Audience
        };
        var token = handler.CreateToken(descriptor);

        user.Password = null;

        return new TokenResponse
        {
            Expires = token.ValidTo,
            Issued = DateTime.UtcNow,
            Token = handler.WriteToken(token),
            User = user,
            Type = "Bearer"
        };        
    }

    public JwtSecurityToken DecodeToken(string token)
        => new JwtSecurityTokenHandler().ReadJwtToken(token);
}