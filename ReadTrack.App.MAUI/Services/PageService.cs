using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using ReadTrack.App.MAUI.Pages;
using ReadTrack.App.MAUI.ViewModels;

namespace ReadTrack.App.MAUI.Services;

public class PageService : BaseService<PageService>, IPageService
{
    private readonly IServiceProvider serviceProvider;
    
    public PageService(ILogger<PageService> logger, IServiceProvider serviceProvider) : base(logger)
        => this.serviceProvider = serviceProvider;

    public async Task GoToAsync(string path)
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync(path);
        }
    }

    public void SwitchToDashboard()
    {
        // TODO: figure out if this is right
        var appCurrent = Application.Current;
        if (appCurrent != null && appCurrent.Windows.Any())
        {
            var viewModel = serviceProvider.GetRequiredService<MenuViewModel>();
            appCurrent.Windows[0].Page = new AppShell(viewModel);
        }
    }

    public void SwitchToLoginPage()
    {
        var appCurrent = Application.Current;
        if (appCurrent != null && appCurrent.Windows.Any())
        {
            var loginViewModel = serviceProvider.GetRequiredService<LoginViewModel>();
            appCurrent.Windows[0].Page = new NavigationPage(new LoginPage(loginViewModel));
        }
    }
}
