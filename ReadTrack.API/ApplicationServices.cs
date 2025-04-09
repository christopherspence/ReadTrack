using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReadTrack.API.Data;

namespace ReadTrack.API;

public static class ApplicationServices
{
    public static async Task CreateOrUpdateDatabaseAsync(this WebApplication app)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider
                .GetRequiredService<ReadTrackContext>();

            await context.Database.MigrateAsync();
            app.Logger.LogInformation("SQL Server database updated");            
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "Error updating database");
            throw;
        }

    }
    
}