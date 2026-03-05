using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserReadRepository
    {
        Task<User?> GetByIdAsync(Guid userId);
        Task<User?> GetByEmailAsync(string email);
    }

    public interface IUserWriteRepository
    {
        Task AddAsync(User user);
    }
}
