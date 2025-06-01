
using System;
using Microsoft.Extensions.DependencyInjection;
using ReadTrack.App.MAUI.Pages;
using ReadTrack.App.MAUI.Services;
using ReadTrack.App.MAUI.ViewModels;
using ReadTrack.Shared.Api;
using Refit;

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
        // TODO: put in appsettings?
        var baseUrl = "https://localhost:5001";

        services.AddRefitClient<IAuthApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl));
    }

    private static void AddViewModels(IServiceCollection services)
    {
        services.AddScoped<LoginViewModel>();
        services.AddScoped<MenuViewModel>();
    }

    private static void AddPages(IServiceCollection services)
    {
        services.AddScoped<DashboardPage>();
        services.AddScoped<LoginPage>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IAlertService, AlertService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<ILocalStorageService, LocalStorageService>();
        services.AddSingleton<IPageService, PageService>();
        services.AddScoped<IUserService, UserService>();
    }
}