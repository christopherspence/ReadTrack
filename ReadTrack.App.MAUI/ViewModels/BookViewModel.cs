using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ReadTrack.App.MAUI.Services;
using ReadTrack.Shared.Models;

namespace ReadTrack.App.MAUI.ViewModels;

public class BookViewModel : BaseViewModel
{
    private ObservableCollection<Book> books = [];
    private readonly IBookService bookService;

    public BookViewModel(IBookService bookService) : base()
        => this.bookService = bookService;

    public ObservableCollection<Book> Books
    {
        get => books;
        set
        {
            books = value;
            RaisePropertyChanged();
        }
    }

    public async Task<ObservableCollection<Book>> GetAsync()
    {
        var books = await bookService.GetBooksAsync();

        Books = new ObservableCollection<Book>(books);

        return Books;
    }
}