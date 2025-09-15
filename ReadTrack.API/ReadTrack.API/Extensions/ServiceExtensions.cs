using System.Security.Claims;
using System.Threading.Tasks;
using ReadTrack.API.Services;
using ReadTrack.Shared;

namespace ReadTrack.API.Extensions;

public static class ServiceExtensions
{
    public static async Task<User?> GetCurrentUserAsync(this IUserService service, ClaimsPrincipal user)
        => await service.GetUserByEmailAsync(user.FindFirstValue(ClaimTypes.Email) ?? string.Empty);
}
