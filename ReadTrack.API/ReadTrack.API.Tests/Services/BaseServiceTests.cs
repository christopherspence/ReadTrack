using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReadTrack.API.Data;
using ReadTrack.API.Models;
using Xunit;

namespace ReadTrack.API.Tests.Services;

public abstract class BaseServiceTests : IAsyncLifetime
{
    private ReadTrackContext? context;
    private IMapper? mapper;
    
    protected ReadTrackContext Context
    {
        get
        {
            if (context == null)
            {
                throw new ArgumentNullException("Context not initialized");
            }
            return context;

        }
    }
    
    protected IMapper Mapper
    {
        get
        {
            if (mapper == null)
            {
                throw new ArgumentNullException("Mapper not initialized");
            }
            return mapper;

        }
    }

    protected virtual async Task SetupDbContextsAsync()
    {
        var builder = new DbContextOptionsBuilder<ReadTrackContext>();
        builder.UseInMemoryDatabase(Guid.NewGuid().ToString());

        var contextInstance = Activator.CreateInstance(typeof(ReadTrackContext), builder.Options);
        if (contextInstance is null)
        {
            throw new InvalidOperationException("Failed to create an instance of ReadTrackContext.");
        }
        context = (ReadTrackContext)contextInstance;
        await Context.Database.EnsureCreatedAsync();
        await Context.Database.EnsureDeletedAsync();
    }

    private void SetupMapper()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        mapper = new Mapper(configuration);
    }

    public async Task InitializeAsync()
    {
        await SetupDbContextsAsync();
        SetupMapper();
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
