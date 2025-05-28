using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReadTrack.API.Data;
using ReadTrack.API.Data.Entities;
using ReadTrack.Shared.Models;
using ReadTrack.Shared.Models.Requests;

namespace ReadTrack.API.Services;

public class SessionService : BaseService<SessionService>, ISessionService
{
    public SessionService(ILogger<SessionService> logger, ReadTrackContext context, IMapper mapper) : base(logger, context, mapper) { }

    private IQueryable<SessionEntity> GetInitialQuery(int userId)
        => Context.Sessions.Where(s => s.UserId == userId && !s.IsDeleted);    

    private IQueryable<SessionEntity> GetInitialQuery(int bookId, int userId)
        => Context.Sessions.Where(s => s.BookId == bookId && s.UserId == userId && !s.IsDeleted);

    public async Task<int> GetSessionCountAsync(int userId, int bookId)
    {
        return await GetInitialQuery(bookId, userId).CountAsync();
    }

    public async Task<IEnumerable<Session>> GetSessionsAsync(int userId, int bookId, int offset, int count)
    {
        var query = GetInitialQuery(bookId, userId).Skip(offset).Take(count);

        return Mapper.Map<IEnumerable<SessionEntity>, IEnumerable<Session>>(await query.ToListAsync());
    }

    public async Task<Session> CreateSessionAsync(int userId, CreateSessionRequest request)
    {
        var entity = new SessionEntity
        {
            BookId = request.BookId,
            NumberOfPages = request.NumberOfPages,
            Date = request.Date,
            Duration = request.Duration,
            StartPage = request.StartPage,
            EndPage = request.EndPage,
            UserId = userId,        
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow
        };

        await Context.Sessions.AddAsync(entity);
        await Context.SaveChangesAsync();

        return Mapper.Map<SessionEntity, Session>(entity);
    }

    public async Task<bool> UpdateSessionAsync(int userId, Session session)
    {
        var existingEntity = await GetInitialQuery(userId).SingleOrDefaultAsync(s => s.Id == session.Id);

        if (existingEntity == null)
        {
            return false;
        }

        existingEntity.NumberOfPages = session.NumberOfPages;
        existingEntity.Duration = session.Duration;
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

        existingEntity.IsDeleted = true;
        existingEntity.Modified = DateTime.UtcNow;

        Context.Entry(existingEntity).State = EntityState.Modified;
        await Context.SaveChangesAsync();

        return true;
    }
}