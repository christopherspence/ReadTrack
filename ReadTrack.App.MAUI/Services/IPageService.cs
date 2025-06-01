using System.Threading.Tasks;

namespace ReadTrack.App.MAUI.Services;

public interface IPageService : IService
{
    Task GoToAsync(string path);
    void SwitchToLoginPage();
    void SwitchToDashboard();    
}