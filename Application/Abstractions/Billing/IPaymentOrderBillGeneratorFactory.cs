using Domain.Enums;

namespace Application.Abstractions.Billing
{
    public interface IPaymentOrderBillGeneratorFactory
    {
        IPaymentOrderBillGenerator CreateForPaymentMean(PaymentMean paymentMean);
    }
}
