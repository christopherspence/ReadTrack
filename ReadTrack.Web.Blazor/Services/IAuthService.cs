namespace ReadTrack.Web.Blazor.Services;

public interface IAuthService : IService
{
    Task<bool> LoginAsync(string email, string password);
}