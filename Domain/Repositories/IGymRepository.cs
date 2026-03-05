using Domain.Entities;
using System.Security.Cryptography;

namespace Domain.Repositories
{
    public interface IGymReadRepository
    {
        Task<Gym?> GetByIdAsync(Guid id);
    }

    public interface IGymWriteRepository
    {
        Task AddAsync(Gym gym);
        Task UpdateAsync(Gym gym);
    }
}
