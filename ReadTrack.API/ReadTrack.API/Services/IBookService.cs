using System.Collections.Generic;
using System.Threading.Tasks;
using ReadTrack.API.Models;

namespace ReadTrack.API.Services;

public interface IBookService : IService 
{ 
    Task<int> GetBookCountAsync(int userId, string searchText = "");

    Task<IEnumerable<Book>> GetBooksAsync(int userId, int offset, int count, string searchText = "");
}