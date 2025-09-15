using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReadTrack.API.Data;
using ReadTrack.API.Data.Entities;
using ReadTrack.Shared;
using ReadTrack.Shared.Requests;

namespace ReadTrack.API.Services;

public class BookService : BaseService<BookService>, IBookService
{
    public BookService(ILogger<BookService> logger, ReadTrackContext context, IMapper mapper) : base(logger, context, mapper)
    {
    }

    private IQueryable<BookEntity> GetInitialQuery(int userId)
        => Context.Books.Where(b => b.UserId == userId && !b.IsDeleted);

    private static IQueryable<BookEntity> AddSearchQuery(IQueryable<BookEntity> query, string searchText)
        => query.Where(b => (b.Name ?? string.Empty).Contains(searchText) || (b.Author ?? string.Empty).Contains(searchText));

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

    public async Task<Book?> GetBookAsync(int userId, int bookId)
    {
        var query = GetInitialQuery(userId);

        var entity = await query.SingleOrDefaultAsync(b => b.Id == bookId);

        if (entity == null)
        {
            return null;
        }

        return Mapper.Map<BookEntity, Book>(entity);
    }

    public async Task<Book> CreateBookAsync(int userId, CreateBookRequest request)
    {
        var entity = new BookEntity
        {
            Name = request.Name ?? string.Empty,
            Author = request.Author ?? string.Empty,
            Category = request.Category,
            NumberOfPages = request.NumberOfPages,
            Published = request.Published,
            UserId = userId,
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow
        };

        await Context.Books.AddAsync(entity);
        await Context.SaveChangesAsync();

        return Mapper.Map<BookEntity, Book>(entity);
    }

    public async Task<bool> UpdateBookAsync(int userId, Book book)
    {
        var existingEntity = await GetInitialQuery(userId).SingleOrDefaultAsync(b => b.Id == book.Id);

        if (existingEntity == null)
        {
            return false;
        }

        existingEntity.Name = book.Name ?? string.Empty;
        existingEntity.Author = book.Author ?? string.Empty;
        existingEntity.Category = book.Category;
        existingEntity.NumberOfPages = book.NumberOfPages;
        existingEntity.Published = book.Published;
        existingEntity.Finished = book.Finished;
        existingEntity.Modified = DateTime.UtcNow;

        Context.Entry(existingEntity).State = EntityState.Modified;
        await Context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteBookAsync(int userId, int bookId)
    {
        var existingEntity = await GetInitialQuery(userId).SingleOrDefaultAsync(b => b.Id == bookId);

        if (existingEntity == null)
        {
            return false;
        }

        existingEntity.IsDeleted = true;
        existingEntity.Modified = DateTime.UtcNow;

        Context.Entry(existingEntity).State = EntityState.Modified;
        await Context.SaveChangesAsync();

        return true;
    }
}