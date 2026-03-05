using Application.Abstractions.UnitOfWork;
using Domain.Entities;
using Domain.Repositories;

namespace Application.UseCases
{
    public class UseInvitationToAdministrateUseCase
    {
        private readonly IInvitationToAdministrateReadRepository _invitationToAdministrateReadRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IGymReadRepository _gymReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UseInvitationToAdministrateUseCase(
            IInvitationToAdministrateReadRepository invitationToAdministrateReadRepository,
            IUserReadRepository userReadRepository,
            IGymReadRepository gymReadRepository,
            IUnitOfWork unitOfWork)
        {
            _invitationToAdministrateReadRepository = invitationToAdministrateReadRepository;
            _userReadRepository = userReadRepository;
            _gymReadRepository = gymReadRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(UseInvitationToAdministrateUseCaseInput input)
        {
            var invitation = await _invitationToAdministrateReadRepository.GetByIdAsync(input.InvitationId);

            if (invitation == null)
                throw new InvalidOperationException("Invitation not found.");

            var gym = await _gymReadRepository.GetByIdAsync(invitation.GymId);

            if (gym == null)
                throw new InvalidOperationException("Gym not found.");

            var user = await _userReadRepository.GetByIdAsync(input.UserId);

            if (user == null)
                throw new InvalidOperationException("User not found.");

            invitation.Use();

            var createAdministratorInput = new CreateAdministratorInput(
                UserId: input.UserId);

            gym.AddAdministrator(createAdministratorInput);

            await _unitOfWork.ExecuteAsync(async writeRepositories =>
            {
                await writeRepositories.InvitationToAdministrateWriteRepository.UpdateAsync(invitation);

                await writeRepositories.GymWriteRepository.UpdateAsync(gym);
            });
        }
    }

    public sealed record UseInvitationToAdministrateUseCaseInput(
        Guid InvitationId,
        Guid UserId);
}
