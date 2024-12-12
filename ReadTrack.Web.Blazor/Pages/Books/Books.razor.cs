using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ReadTrack.Web.Blazor.Pages.Books;

public partial class BookListComponent : ComponentBase
{
    /*[Inject] private BookService Service { get; set; }
    [Inject] private ISnackBar SnackBar { get; set; }
    [Inject] private IDialogService Dialog { get; set; }

    private int limit = 10;
    private int offset = 0;
    private int count = 0;
    private string searchValue = string.Empty;

    private List<Book> books = new List<Book>();

    protected override async Task OnInitializedAsync()
    {
        await GetBooks();
    }

    private async Task GetBooks()
    {
        count = await Service.GetBookCount(searchValue);
        books = await Service.GetBooks(offset * limit, limit, searchValue);
    }

    private async Task CreateBook()
    {
        var dialogRef = Dialog.Open<AddEditBookDialogComponent>("Add Book", new DialogParameters { { "mode", DialogMode.Add } });
        var result = await dialogRef.Result;

        if (!result.Cancelled)
        {
            await GetBooks();
        }
    }

    private async Task EditBook(Book row)
    {
        var book = books.Find(b => b.Id == row.Id);
        var dialogRef = Dialog.Open<AddEditBookDialogComponent>("Edit Book", new DialogParameters { { "mode", DialogMode.Edit }, { "book", book } });
        var result = await dialogRef.Result;

        if (!result.Cancelled)
        {
            await GetBooks();
        }
    }

    private async Task DeleteBook(Book row)
    {
        var book = books.Find(b => b.Id == row.Id);
        var dialogRef = Dialog.Open<ConfirmDialogComponent>("Confirmation", new DialogParameters
        {
            { "title", "Confirmation" },
            { "message", $"Are you sure you wish to delete {book?.Name}?" },
            { "destructive", true },
            { "confirmText", "Delete" }
        });

        var result = await dialogRef.Result;

        if (!result.Cancelled)
        {
            var response = await Service.DeleteBook(row.Id);
            if (response == null)
            {
                SnackBar.Show("Book Deleted", row.Id.ToString(), new SnackBarOptions { Duration = 2000, VerticalPosition = VerticalPosition.Top, HorizontalPosition = HorizontalPosition.Right });
                await GetBooks();
            }
            else
            {
                Dialog.Open<SimpleDialogComponent>("Error", new DialogParameters { { "title", "Error" }, { "message", "An error occurred deleting this book" } });
            }
        }
    }

    private async Task UpdateFilter(string searchText)
    {
        searchValue = searchText;
        await GetBooks();
    }

    private async Task OnPage(PageEventArgs eventArgs)
    {
        limit = eventArgs.Limit;
        offset = eventArgs.Offset;
        await GetBooks();
    }

    private async Task ToggleFinished(ChangeEventArgs eventArgs)
    {
        var book = books.Find(b => b.Id == Convert.ToInt32(eventArgs.Value));
        
        if (book != null)
        {
            book.Finished = eventArgs.Checked;
            await Service.UpdateBook(book);
        }
    }*/
}

