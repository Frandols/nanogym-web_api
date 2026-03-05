using Application.Abstractions.UnitOfWork;
using Domain.Repositories;

namespace Application.UseCases
{
    public class InviteAdministratorUseCase
    {
        private readonly IGymReadRepository _gymReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InviteAdministratorUseCase(
            IGymReadRepository gymReadRepository,
            IUnitOfWork unitOfWork)
        {
            _gymReadRepository = gymReadRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> ExecuteAsync(InviteAdministratorUseCaseInput input)
        {
            var gym = await _gymReadRepository.GetByIdAsync(input.GymId);

            if (gym == null)
                throw new InvalidOperationException("Gym not found.");

            if(input.InviterId != gym.OwnerId)
                throw new InvalidOperationException("Only the gym owner can invite administrators.");

            var invitation = gym.GenerateInvitationToAdministrate();

            await _unitOfWork.ExecuteAsync(async writeRepositories =>
            {
                await writeRepositories.GymWriteRepository.UpdateAsync(gym);
            });
                
            return invitation.Id;
        }
    }

    public sealed record InviteAdministratorUseCaseInput(
        Guid GymId,
        Guid InviterId);
}
