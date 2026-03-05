using Domain.Entities;

namespace Domain.Repositories
{
    public interface ISubscriptionReadRepository
    {
        Task<Subscription?> GetByIdAsync(Guid subscriptionId);
    }

    public interface ISubscriptionWriteRepository
    {
        Task AddAsync(Subscription subscription);
    }
}
