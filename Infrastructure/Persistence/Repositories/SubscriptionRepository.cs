using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class SubscriptionReadRepository : ISubscriptionReadRepository
    {
        private readonly ApplicationDbContext _db;
        
        public SubscriptionReadRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public async Task<Subscription?> GetByIdAsync(Guid subscriptionId)
        {
            return await _db.Subscriptions
                .Where(subscription => subscription.Id == subscriptionId)
                .FirstOrDefaultAsync();
        }
    }

    public class SubscriptionWriteRepository : ISubscriptionWriteRepository
    {
        private readonly ApplicationDbContext _db;
        
        public SubscriptionWriteRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public Task AddAsync(Subscription subscription)
        {
            _db.Subscriptions.Add(subscription);
            return Task.CompletedTask;
        }
    }
}
