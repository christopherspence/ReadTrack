using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReadTrack.API.Data;
using ReadTrack.API.Data.Entities;
using ReadTrack.API.Models;

namespace ReadTrack.API.Services;

public class BookService : BaseService<BookService>, IBookService
{
    public BookService(ILogger<BookService> logger, ReadTrackContext context, IMapper mapper) : base(logger, context, mapper)
    {
    }

    private IQueryable<BookEntity> GetInitialQuery(int userId)
        => Context.Books.Where(b => b.UserId == userId && !b.IsDeleted);

    private IQueryable<BookEntity> AddSearchQuery(IQueryable<BookEntity> query, string searchText)
        => query.Where(b => b.Name.Contains(searchText) ||b.Author.Contains(searchText));

    public async Task<int> GetBookCountAsync(int userId, string searchText = "")
    {
        var query = GetInitialQuery(userId);

        if (!string.IsNullOrEmpty(searchText))
        {
            query = AddSearchQuery(query, searchText);
        }

        return await query.CountAsync();
    }

    public async Task<IEnumerable<Book>> GetBooksAsync(int userId, int offset, int count, string searchText)
    {
        var query = GetInitialQuery(userId);

        if (!string.IsNullOrEmpty(searchText))
        {
            query = AddSearchQuery(query, searchText);
        }

        return Mapper.Map<IEnumerable<BookEntity>, IEnumerable<Book>>(await query.Skip(offset).Take(count).ToListAsync());
    }
}