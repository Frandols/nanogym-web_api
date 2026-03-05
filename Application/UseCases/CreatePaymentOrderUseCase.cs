using Application.Abstractions.UnitOfWork;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;

namespace Application.UseCases
{
    public class CreatePaymentOrderUseCase
    {
        private readonly ISubscriptionReadRepository _subscriptionReadRepository;
        private readonly IGymReadRepository _gymReadRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePaymentOrderUseCase(
            ISubscriptionReadRepository subscriptionReadRepository,
            IGymReadRepository gymReadRepository,
            IUserReadRepository userReadRepository,
            IUnitOfWork unitOfWork)
        {
            _subscriptionReadRepository = subscriptionReadRepository;
            _gymReadRepository = gymReadRepository;
            _userReadRepository = userReadRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> ExecuteAsync(CreatePaymentOrderUseCaseInput input)
        {
            var subscription = await _subscriptionReadRepository.GetByIdAsync(input.SubscriptionId);

            if (subscription == null)
                throw new InvalidOperationException("Subscription not found.");

            var gym = await _gymReadRepository.GetByIdAsync(subscription.GymId);

            if (gym == null)
                throw new InvalidOperationException("Gym not found.");

            var owner = await _userReadRepository.GetByIdAsync(gym.OwnerId);

            if (owner == null)
                throw new InvalidOperationException("Owner not found.");

            if(!owner.CanBePaidBy(input.PaymentMean))
                throw new InvalidCastException("The owner cannot be paid by the specified payment mean.");
            
            var createPaymentOrderInput = new CreatePaymentOrderInput(
                Amount: input.Amount,
                PaymentMean: input.PaymentMean);

            var paymentOrder = new PaymentOrder(createPaymentOrderInput);

            await _unitOfWork.ExecuteAsync(async writeRepositories =>
            {
                await writeRepositories.PaymentOrderWriteRepository.AddAsync(paymentOrder);
            });

            return paymentOrder.Id;
        }
    }

    public sealed record CreatePaymentOrderUseCaseInput(
        Guid SubscriptionId,
        decimal Amount,
        PaymentMean PaymentMean);
}
