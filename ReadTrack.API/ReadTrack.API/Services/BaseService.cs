using AutoMapper;
using Microsoft.Extensions.Logging;
using ReadTrack.API.Data;

namespace ReadTrack.API.Services;

public abstract class BaseService<T> where T : class
{
    protected ILogger<T> Logger { get; private set; }
    protected ReadTrackContext Context { get; private set; }
    protected IMapper Mapper { get; private set; }
}