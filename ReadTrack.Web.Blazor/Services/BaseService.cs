using Microsoft.Extensions.Logging;

namespace ReadTrack.Web.Blazor.Services;

public abstract class BaseService<T> where T : class
{
    public BaseService(ILogger<T> logger)
        => Logger = logger;    

    protected ILogger<T> Logger { get; private set; }
}