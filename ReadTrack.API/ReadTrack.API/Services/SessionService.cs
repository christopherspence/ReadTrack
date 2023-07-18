using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReadTrack.API.Data;
using ReadTrack.API.Data.Entities;
using ReadTrack.API.Models;
using ReadTrack.API.Models.Requests;

namespace ReadTrack.API.Services;

public class SessionService : BaseService<SessionService>, ISessionService
{
    private readonly IBookService bookService;

    public SessionService(ILogger<SessionService> logger, ReadTrackContext context, IBookService bookService, IMapper mapper) : base(logger, context, mapper)
        => this.bookService = bookService;

    private IQueryable<SessionEntity> GetInitialQuery(int bookId)
        => Context.Sessions.Where(b => b.BookId == bookId && !b.IsDeleted);

    public async Task<int> GetSessionCountAsync(int userId, int bookId)
    {
        // Make sure the book exists and belongs to the user
        var book = bookService.GetBookAsync(userId, bookId);

        if (book == null)
        {
            throw new ApplicationException("Book does not exist");
        }

        return await GetInitialQuery(bookId).CountAsync();
    }

    public async Task<IEnumerable<Session>> GetSessionsAsync(int userId, int bookId, int offset, int count)
    {
        // Make sure the book exists and belongs to the user
        var book = bookService.GetBookAsync(userId, bookId);

        if (book == null)
        {
            throw new ApplicationException("Book does not exist");
        }

        var query = GetInitialQuery(bookId).Skip(offset).Take(count);

        return Mapper.Map<IEnumerable<SessionEntity>, IEnumerable<Session>>(await query.ToListAsync());
    }

    public async Task<Session> CreateSessionAsync(int userId, CreateSessionRequest request)
    {
        // Make sure the book exists and belongs to the user
        var book = bookService.GetBookAsync(userId, request.BookId);

        if (book == null)
        {
            throw new ApplicationException("Book does not exist");
        }

        var entity = new SessionEntity
        {
            BookId = request.BookId,
            NumberOfPages = request.NumberOfPages,
            Time = request.Time,
            StartPage = request.StartPage,
            EndPage = request.EndPage,        
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow
        };

        await Context.Sessions.AddAsync(entity);
        await Context.SaveChangesAsync();

        return Mapper.Map<SessionEntity, Session>(entity);
    }

    public async Task<bool> UpdateSessionAsync(int userId, Session session)
    {
        var existingEntity = await GetInitialQuery(userId).SingleOrDefaultAsync(b => b.Id == session.Id);

        // make sure the book belongs to the user
        var book = bookService.GetBookAsync(userId, existingEntity.BookId);

        if (book == null)
        {
            throw new ApplicationException("Book does not exist");
        }

        if (existingEntity == null)
        {
            return false;
        }

        existingEntity.NumberOfPages = session.NumberOfPages;
        existingEntity.Time = session.Time;
        existingEntity.StartPage = session.StartPage;
        existingEntity.EndPage = session.EndPage;        
        existingEntity.Created = DateTime.UtcNow;
            
        existingEntity.Modified = DateTime.UtcNow;

        Context.Entry(existingEntity).State = EntityState.Modified;
        await Context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteSessionAsync(int userId, int sessionId)
    {
        var existingEntity = await GetInitialQuery(userId).SingleOrDefaultAsync(b => b.Id == sessionId);

        // make sure the book belongs to the user
        var book = bookService.GetBookAsync(userId, existingEntity.BookId);

        if (book == null)
        {
            throw new ApplicationException("Book does not exist");
        }

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