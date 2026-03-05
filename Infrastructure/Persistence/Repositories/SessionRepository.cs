using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class SessionReadRepository : ISessionReadRepository
    {
        private readonly ApplicationDbContext _db;
        
        public SessionReadRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public async Task<Session?> GetByIdAsync(Guid sessionId)
        {
            return await _db.Sessions
                .Where(session => session.Id == sessionId)
                .FirstOrDefaultAsync();
        }
        
        public async Task<Session?> GetByTokenHashAsync(string tokenHash)
        {
            return await _db.Sessions
                .Where(session => session.TokenHash == tokenHash)
                .FirstOrDefaultAsync();
        }
    }

    public class SessionWriteRepository : ISessionWriteRepository
    {
        private readonly ApplicationDbContext _db;

        public SessionWriteRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task AddAsync(Session session)
        {
            _db.Sessions.Add(session);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(Session session)
        {
            _db.Sessions.Update(session);

            return Task.CompletedTask;
        }
    }
}
