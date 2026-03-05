using Application.Abstractions.Billing;
using Domain.Repositories;

namespace Application.UseCases
{
    public class GeneratePaymentOrderBillUseCase
    {
        private readonly IPaymentOrderReadRepository _paymentOrderReadRepository;
        private readonly IPaymentOrderBillGeneratorFactory _paymentOrderBillGeneratorFactory;

        public GeneratePaymentOrderBillUseCase(
            IPaymentOrderReadRepository paymentOrderReadRepository,
            IPaymentOrderBillGeneratorFactory paymentOrderBillGeneratorFactory)
        {
            _paymentOrderReadRepository = paymentOrderReadRepository;
            _paymentOrderBillGeneratorFactory = paymentOrderBillGeneratorFactory;
        }

        public async Task<PaymentOrderBill> ExecuteAsync(GeneratePaymentOrderBillUseCaseInput input)
        {
            var paymentOrder = await _paymentOrderReadRepository.GetByIdAsync(input.PaymentOrderId);

            if (paymentOrder == null)
                throw new InvalidOperationException("Payment order not found.");

            var paymentOrderBillGenerator = _paymentOrderBillGeneratorFactory.CreateForPaymentMean(paymentOrder.PaymentMean);

            var paymentOrderBill = await paymentOrderBillGenerator.ExecuteAsync(paymentOrder);

            return paymentOrderBill;
        }
    }

    public sealed record GeneratePaymentOrderBillUseCaseInput(
        Guid PaymentOrderId);
}
