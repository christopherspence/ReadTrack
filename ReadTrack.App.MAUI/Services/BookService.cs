using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReadTrack.Shared.Api;
using ReadTrack.Shared.Models;

namespace ReadTrack.App.MAUI.Services;

public class BookService : BaseService<BookService, IBookApi>, IBookService
{
    public BookService(ILogger<BookService> logger, IBookApi api) : base(logger, api)
    {
    }

    // TODO: eventually want to support endless scrolling, but for now, just get all of them
    public async Task<IEnumerable<Book>> GetBooksAsync()
    {
        var count = await Api.GetBookCountAsync();
        var books = new List<Book>();

        for (var i = 0; i < count; i += 10)
        {
            var batch = await Api.GetBooksAsync(i, 10);
            books.AddRange(batch);            
        }

        return books;
    }

    public Task<Book> GetBookAsync(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<Book> CreateBookAsync(Book book)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteBookAsync(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> UpdateBookAsync(Book book)
    {
        throw new System.NotImplementedException();
    }
}
