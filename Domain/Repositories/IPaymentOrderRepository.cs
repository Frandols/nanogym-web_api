using Domain.Entities;

namespace Domain.Repositories
{
    public interface IPaymentOrderReadRepository
    {
        Task<PaymentOrder?> GetByIdAsync(Guid paymentOrderId);
    }

    public interface IPaymentOrderWriteRepository
    {
        Task AddAsync(PaymentOrder paymentOrder);
    }
}
