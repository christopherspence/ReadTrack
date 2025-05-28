using System.Collections.Generic;
using System.Threading.Tasks;
using ReadTrack.Shared.Models;
using ReadTrack.Shared.Models.Requests;

namespace ReadTrack.API.Services;

public interface ISessionService : IService 
{ 
    Task<int> GetSessionCountAsync(int userId, int bookId);
    Task<IEnumerable<Session>> GetSessionsAsync(int userId, int bookId, int offset, int count);
    Task<Session> CreateSessionAsync(int userId, CreateSessionRequest request);
    Task<bool> UpdateSessionAsync(int userId, Session session);
    Task<bool> DeleteSessionAsync(int userId, int sessionId);
}