using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadTrack.Services;

namespace ReadTrack.Controllers;

[ApiController]
public abstract class BaseController<C, S> : ControllerBase
    where C : ControllerBase
    where S : IService
{
    protected ILogger<C> Logger { get; private set; }
    protected S Service { get; private set; }

    public BaseController(ILogger<C> logger, S service)
    {
        Logger = logger;
        Service = service;
    }
}