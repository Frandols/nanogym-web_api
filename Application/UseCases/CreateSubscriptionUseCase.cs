using Application.Abstractions.UnitOfWork;
using Domain.Entities;
using Domain.Repositories;

namespace Application.UseCases
{
    public class CreateSubscriptionUseCase
    {
        private readonly IGymReadRepository _gymReadRepository;
        private readonly IMemberReadRepository _memberReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSubscriptionUseCase(
            IGymReadRepository gymReadRepository,
            IMemberReadRepository memberReadRepository,
            IUnitOfWork unitOfWork)
        {
            _gymReadRepository = gymReadRepository;
            _memberReadRepository = memberReadRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> ExecuteAsync(CreateSubscriptionUseCaseInput input)
        {
            var gym = await _gymReadRepository.GetByIdAsync(input.GymId);

            if(gym == null)
                throw new InvalidOperationException("Gym not found.");

            var plan = gym.GetPlanWithId(input.PlanId);

            var member = await _memberReadRepository.GetByIdAsync(input.MemberId);

            if(member == null)
                throw new InvalidOperationException("Member not found.");

            var createSubscriptionInput = new CreateSubscriptionInput(
                MemberId: input.MemberId,
                Plan: plan,
                GymId: input.GymId,
                StartsAt: input.StartsAt);

            var subscription = new Subscription(createSubscriptionInput);

            await _unitOfWork.ExecuteAsync(async writeRepositories =>
            {
                await writeRepositories.SubscriptionWriteRepository.AddAsync(subscription);
            });

            return subscription.Id;
        }
    }

    public sealed record CreateSubscriptionUseCaseInput(
        Guid GymId,
        Guid PlanId,
        Guid MemberId,
        DateTime StartsAt);
}
