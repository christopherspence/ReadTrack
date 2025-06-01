using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ReadTrack.App.MAUI.Services;

public class LocalStorageService : BaseService<LocalStorageService>, ILocalStorageService
{
#if DEBUG
    private readonly Dictionary<string, string> items = [];

#endif

    public LocalStorageService(ILogger<LocalStorageService> logger) : base(logger)
    {
    }


    public async Task<string?> GetAsync(string key)
    {
#if DEBUG
        if (items.Any(k => k.Key == key))
        {
            return items[key];
        }

        Logger.LogWarning("Key {key} not found in storage.", key);    
        return null;
#else
        return await SecureStorage.GetAsync(key);
#endif
    }

    public async Task SetAsync(string key, string value)
    {
#if DEBUG
        items[key] = value;
#else
        await SecureStorage.SetAsync(key, value);
#endif
    }
    
    
}