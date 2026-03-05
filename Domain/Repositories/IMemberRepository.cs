using Domain.Entities;

namespace Domain.Repositories
{
    public interface IMemberReadRepository
    {
        Task<Member?> GetByIdAsync(Guid id);
    }

    public interface IMemberWriteRepository
    {
        Task AddAsync(Member member);
    }
}
