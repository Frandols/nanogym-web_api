using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class PaymentOrderReadRepository : IPaymentOrderReadRepository
    {
        private readonly ApplicationDbContext _db;
        
        public PaymentOrderReadRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public async Task<PaymentOrder?> GetByIdAsync(Guid paymentOrderId)
        {
            return await _db.PaymentOrders
                .Where(paymentOrder => paymentOrder.Id == paymentOrderId)
                .FirstOrDefaultAsync();
        }
    }

    public class PaymentOrderWriteRepository : IPaymentOrderWriteRepository
    {
        private readonly ApplicationDbContext _db;
        
        public PaymentOrderWriteRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task AddAsync(PaymentOrder paymentOrder)
        {
            _db.PaymentOrders.Add(paymentOrder);

            return Task.CompletedTask;
        }
    }
}
