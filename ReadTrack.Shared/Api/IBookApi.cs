using System.Collections.Generic;
using System.Threading.Tasks;
using ReadTrack.Shared.Models;
using Refit;

namespace ReadTrack.Shared.Api;

public interface IBookApi : IAuthenticatedApi
{
    [Get("/api/book/count/{searchText}")]    
    Task<int> GetBookCountAsync(string searchText = "");

    [Get("/api/book/{offset}/{count}/{searchText}")]    
    Task<IEnumerable<Book>> GetBooksAsync(int offset, int count, string searchText = "");
}