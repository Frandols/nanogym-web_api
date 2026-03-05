using Application.Abstractions.UnitOfWork;
using Domain.Entities;
using Domain.Repositories;

namespace Application.UseCases
{
    public class CreateMemberUseCase
    {
        private readonly IGymReadRepository _gymReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMemberUseCase(
            IGymReadRepository gymReadRepository,
            IUnitOfWork unitOfWork)
        {
            _gymReadRepository = gymReadRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> ExecuteAsync(CreateMemberUseCaseInput input)
        {
            var gym = await _gymReadRepository.GetByIdAsync(input.GymId);

            if(gym == null)
                throw new InvalidOperationException("Gym not found.");

            var createMemberInput = new CreateMemberInput(
                GymId: input.GymId,
                Name: input.Name,
                Email: input.Email);

            var member = new Member(createMemberInput);

            await _unitOfWork.ExecuteAsync(async writeRepositories =>
            {
                await writeRepositories.MemberWriteRepository.AddAsync(member);
            });

            return member.Id;
        }
    }

    public sealed record CreateMemberUseCaseInput(
        Guid GymId,
        string Name,
        string Email);
}
