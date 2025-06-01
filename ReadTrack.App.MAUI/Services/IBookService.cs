using System.Collections.Generic;
using System.Threading.Tasks;
using ReadTrack.Shared.Models;

namespace ReadTrack.App.MAUI.Services;

public interface IBookService : IService
{
    Task<IEnumerable<Book>> GetBooksAsync();
    Task<Book> GetBookAsync(int id);
    Task<Book> CreateBookAsync(Book book);    
    Task<bool> UpdateBookAsync(Book book);
    Task DeleteBookAsync(int id);
}