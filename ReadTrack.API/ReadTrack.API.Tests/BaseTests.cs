using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReadTrack.API.Data;
using ReadTrack.API.Models;
using Xunit;

namespace ReadTrack.API.Tests;

public abstract class BaseTests : IAsyncLifetime
{
    protected ReadTrackContext Context { get; private set; }
    protected IMapper Mapper { get; private set; }

    protected virtual async Task SetupDbContextsAsync()
    {
        var builder = new DbContextOptionsBuilder<ReadTrackContext>();
        builder.UseInMemoryDatabase(Guid.NewGuid().ToString());

        Context = (ReadTrackContext)Activator.CreateInstance(typeof(ReadTrackContext), builder.Options);
        await Context.Database.EnsureCreatedAsync();
        await Context.Database.EnsureDeletedAsync();
    }

    private void SetupMapper()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        Mapper = new Mapper(configuration);
    }

    public async Task InitializeAsync()
    {
        await SetupDbContextsAsync();
        SetupMapper();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    
}
