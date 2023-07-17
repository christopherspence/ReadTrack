using System.Collections.Generic;
using System.Threading.Tasks;
using ReadTrack.API.Models;
using ReadTrack.API.Models.Requests;

namespace ReadTrack.API.Services;

public interface IBookService : IService 
{ 
    Task<int> GetBookCountAsync(int userId, string searchText = "");
    Task<IEnumerable<Book>> GetBooksAsync(int userId, int offset, int count, string searchText = "");
    Task<Book> CreateBookAsync(int userId, CreateBookRequest request);
    Task<bool> UpdateBookAsync(int userId, Book book);
    Task<bool> DeleteBookAsync(int userId, int bookId);
}