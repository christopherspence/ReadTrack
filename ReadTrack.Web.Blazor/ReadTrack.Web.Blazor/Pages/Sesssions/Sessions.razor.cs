using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ReadTrack.Web.Blazor.Pages.Sessions;

public partial class SessionListComponent : ComponentBase
{
    [Inject] private IActivatedRoute Route { get; set; }
    [Inject] private ISessionService Service { get; set; }
    [Inject] private IDialogService Dialog { get; set; }
    [Inject] private ISnackBarService SnackBar { get; set; }

    private int id;
    private int limit = 10;
    private int offset = 0;
    private int count = 0;
    private string searchValue = string.Empty;

    private List<Session> sessions = new List<Session>();

    protected override async Task OnInitializedAsync()
    {
        id = int.Parse(Route.Parameters["id"]);
        await GetSessions();
    }

    private async Task GetSessions()
    {
        count = await Service.GetSessionCount(id, searchValue) ?? 0;
        sessions = await Service.GetSessions(id, offset * limit, limit, searchValue) ?? new List<Session>();
    }

    private async Task CreateSession()
    {
        var dialogRef = Dialog.Open<AddEditSessionDialogComponent>(new DialogOptions
        {
            Width = "50%",
            Data = new { BookId = id, Mode = DialogMode.Add }
        });

        var result = await dialogRef.Result;

        if (result != null)
        {
            await GetSessions();
        }
    }

    private async Task EditSession(Session row)
    {
        var session = sessions.Find(s => s.Id == row.Id);
        var dialogRef = Dialog.Open<AddEditSessionDialogComponent>(new DialogOptions
        {
            Width = "50%",
            Data = new { Mode = DialogMode.Edit, Session = session }
        });

        var result = await dialogRef.Result;

        if (result != null)
        {
            await GetSessions();
        }
    }

    private async Task DeleteSession(Session row)
    {
        var session = sessions.Find(s => s.Id == row.Id);
        var dialogRef = Dialog.Open<ConfirmDialogComponent>(new DialogOptions
        {
            Width = "300px",
            Data = new
            {
                Title = "Confirmation",
                Message = $"Are you sure you wish to delete session {session?.Id}?",
                Destructive = true,
                ConfirmText = "Delete"
            }
        });

        var result = await dialogRef.Result;

        if (result != null)
        {
            var response = await Service.DeleteSession(row.Id);
            if (response == null)
            {
                SnackBar.Open("Session Deleted", row.Id.ToString(), new SnackBarOptions
                {
                    Duration = 2000,
                    VerticalPosition = SnackBarVerticalPosition.Top,
                    HorizontalPosition = SnackBarHorizontalPosition.Right
                });

                await GetSessions();
            }
            else
            {
                Dialog.Open<SimpleDialogComponent>(new DialogOptions
                {
                    Width = "250px",
                    Data = new
                    {
                        Title = "Error",
                        Message = "An error occurred deleting this session"
                    }
                });
            }
        }
    }

    private async Task UpdateFilter(string searchText)
    {
        searchValue = searchText;
        await GetSessions();
    }

    private async Task OnPage(PageEventArgs eventArgs)
    {
        limit = eventArgs.Limit;
        offset = eventArgs.Offset;
        await GetSessions();
    }
}

