using Application.Abstractions.UnitOfWork;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.UseCases
{
    public class CreateGymUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateGymUseCase(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> ExecuteAsync(CreateGymUseCaseInput input)
        {
            var createGymInput = new CreateGymInput(
                OwnerId: input.UserId,
                Name: input.Name,
                Location: input.Location);

            var gym = new Gym(createGymInput);

            await _unitOfWork.ExecuteAsync(async writeRepositories =>
            {
                await writeRepositories.GymWriteRepository.AddAsync(gym);
            });

            return gym.Id;
        }
    }

    public sealed record CreateGymUseCaseInput(
        Guid UserId,
        string Name,
        Location Location);
}
