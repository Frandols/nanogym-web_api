using Domain.Entities;

namespace Domain.Repositories
{
    public interface IInvitationToAdministrateReadRepository
    {
        Task<InvitationToAdministrate?> GetByIdAsync(Guid id);
    }

    public interface IInvitationToAdministrateWriteRepository
    {
        Task UpdateAsync(InvitationToAdministrate invitationToAdministrate);
    }
}
