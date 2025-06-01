
using System;
using Microsoft.Extensions.DependencyInjection;
using ReadTrack.App.MAUI.Pages;
using ReadTrack.App.MAUI.Services;
using ReadTrack.App.MAUI.Utilities;
using ReadTrack.App.MAUI.ViewModels;
using ReadTrack.Shared.Api;
using Refit;
using ReadTrackApp = Microsoft.Maui.Controls.Application;

namespace ReadTrack.App.MAUI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServicesToDI(this IServiceCollection services)
    {
        AddRefitContracts(services);
        AddViewModels(services);
        AddPages(services);
        AddServices(services);
    }

    private static void AddRefitContracts(IServiceCollection services)
    {
        AddRefitClient<IAuthApi>(services);
        AddRefitClient<IBookApi>(services, true);
    }

    private static void AddRefitClient<T>(IServiceCollection services, bool useToken = false) where T : class, IApi
    {
        // TODO: put in appsettings?
        var baseUrl = "https://localhost:5001";
        
        RefitSettings settings;

        if (useToken)
        {
            settings = new()
            {
                AuthorizationHeaderValueGetter = async (message, cancellationToken) =>
                {
                    // TODO: figure out why this doesn't work with user service
                    var localStorageService = (ReadTrackApp.Current as App)!.Services.GetRequiredService<ILocalStorageService>();
                    var token = await localStorageService.GetAsync(Constants.UserToken);
                    return token ?? string.Empty;
                }
            };
        }
        else
        {
            settings = new();
        }

        services.AddRefitClient<T>(settings).ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl));
    }

    private static void AddViewModels(IServiceCollection services)
    {
        services.AddScoped<BookViewModel>();
        services.AddScoped<LoginViewModel>();
        services.AddScoped<MenuViewModel>();
    }

    private static void AddPages(IServiceCollection services)
    {
        services.AddScoped<BooksPage>();
        services.AddScoped<DashboardPage>();
        services.AddScoped<LoginPage>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IAlertService, AlertService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<ILocalStorageService, LocalStorageService>();
        services.AddSingleton<IPageService, PageService>();
        services.AddScoped<IUserService, UserService>();
    }
}