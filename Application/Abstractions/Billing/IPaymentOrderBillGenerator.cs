using Domain.Entities;

namespace Application.Abstractions.Billing
{
    public interface IPaymentOrderBillGenerator
    {
        Task<PaymentOrderBill> ExecuteAsync(PaymentOrder paymentOrder, CancellationToken cancellationToken = default);
    }
}
