using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class InvitationToAdministrateReadRepository : IInvitationToAdministrateReadRepository
    {
        private readonly ApplicationDbContext _db;
        
        public InvitationToAdministrateReadRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public async Task<InvitationToAdministrate?> GetByIdAsync(Guid invitationToAdministrateId)
        {
            return await _db.InvitationsToAdministrate
                .Where(invitationToAdministrate => invitationToAdministrate.Id == invitationToAdministrateId)
                .FirstOrDefaultAsync();
        }
    }

    public class InvitationToAdministrateWriteRepository : IInvitationToAdministrateWriteRepository
    {
        private readonly ApplicationDbContext _db;
        
        public InvitationToAdministrateWriteRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public Task UpdateAsync(InvitationToAdministrate invitationToAdministrate)
        {
            _db.InvitationsToAdministrate.Update(invitationToAdministrate);

            return Task.CompletedTask;
        }
    }
}
