using System.Diagnostics;
using Microsoft.AspNetCore.Components.Forms;
using ReadTrack.Shared.Requests;

namespace ReadTrack.Web.Blazor.Components.Pages;

public partial class Login
{
    private AuthRequest authRequest = new();

    async Task HandleLogin()
    {
        Debugger.Break();
    }
}