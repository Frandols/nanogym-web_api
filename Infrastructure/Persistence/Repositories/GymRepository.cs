using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class GymReadRepository : IGymReadRepository
    {
        private readonly ApplicationDbContext _db;
        
        public GymReadRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public async Task<Gym?> GetByIdAsync(Guid gymId)
        {
            return await _db.Gyms
                .Where(gym => gym.Id == gymId)
                .FirstOrDefaultAsync();
        }
    }

    public class GymWriteRepository : IGymWriteRepository
    {
        private readonly ApplicationDbContext _db;
        
        public GymWriteRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public Task AddAsync(Gym gym)
        {
            _db.Gyms.Add(gym);

            return Task.CompletedTask;
        }
        
        public Task UpdateAsync(Gym gym)
        {
            _db.Gyms.Update(gym);

            return Task.CompletedTask;
        }
    }
}
