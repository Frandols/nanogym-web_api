using Domain.Entities;

namespace Domain.Repositories
{
    public interface ISessionReadRepository
    {
        Task<Session?> GetByIdAsync(Guid sessionId);
        Task<Session?> GetByTokenHashAsync(string tokenHash);
    }

    public interface ISessionWriteRepository
    {
        Task AddAsync(Session session);
    }
}
