using Microsoft.Extensions.Logging;
using ReadTrack.Shared.Api;

namespace ReadTrack.App.MAUI.Services;

public abstract class BaseService<C> 
    where C : class, IService
    
{
    public BaseService(ILogger<C> logger) => Logger = logger;

    protected ILogger<C> Logger { get; private set; }    
}

public abstract class BaseService<C, A> : BaseService<C>
    where C : class, IService
    where A : IApi
{
    public BaseService(ILogger<C> logger, A api)
        : base(logger)
        => Api = api;
            
    protected A Api { get; private set; }
}