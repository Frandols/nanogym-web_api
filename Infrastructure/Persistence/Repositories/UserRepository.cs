using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserReadRepository : IUserReadRepository
    {
        private readonly ApplicationDbContext _db;
        
        public UserReadRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await _db.Users
                .Where(user => user.Id == userId)
                .FirstOrDefaultAsync();
        }
        
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users
                .Where(user => user.Email == email)
                .FirstOrDefaultAsync();
        }
    }

    public class UserWriteRepository : IUserWriteRepository
    {
        private readonly ApplicationDbContext _db;
        
        public UserWriteRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public Task AddAsync(User user)
        {
            _db.Users.Add(user);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(User user)
        {
            _db.Users.Update(user);
            return Task.CompletedTask;
        }
    }
}
