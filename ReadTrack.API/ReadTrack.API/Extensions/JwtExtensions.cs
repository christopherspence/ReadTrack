using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ReadTrack.API.Models;

namespace ReadTrack.API.Extensions;

public static class JwtExtensions
{
    public static void AddJwt(this IServiceCollection services, IConfiguration configuration, JwtSettings settings)
    {
        services.AddAuthentication().AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                //ValidateAudience = false,
                ValidAudience = settings.Audience,
                ValidIssuer = settings.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(WebEncoders.Base64UrlDecode(settings.SecretKey))
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("authPolicy", policy => policy.RequireAuthenticatedUser());
        });
    }
}
