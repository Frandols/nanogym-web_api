using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class MemberReadRepository : IMemberReadRepository
    {
        private readonly ApplicationDbContext _db;
        
        public MemberReadRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public async Task<Member?> GetByIdAsync(Guid memberId)
        {
            return await _db.Members
                .Where(member => member.Id == memberId)
                .FirstOrDefaultAsync();
        }
    }

    public class MemberWriteRepository : IMemberWriteRepository
    {
        private readonly ApplicationDbContext _db;
        
        public MemberWriteRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public Task AddAsync(Member member)
        {
            _db.Members.AddAsync(member);

            return Task.CompletedTask;
        }
    }
}
