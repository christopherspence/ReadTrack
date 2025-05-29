namespace ReadTrack.App.MAUI.Services;

public interface ILocalStorageService : IService
{
    Task<string?> GetAsync(string key);

    Task SetAsync(string key, string value);
}