using ReadTrack.Shared.Models;

namespace ReadTrack.App.MAUI.ViewModels;

public class UserViewModel : BaseViewModel
{
    private User? user;

    public User? User
    {
        get => user;
        set
        {
            user = value;
            RaisePropertyChanged();
        }
    }
}