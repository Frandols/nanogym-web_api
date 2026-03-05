using Application.Abstractions.UnitOfWork;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure.UnitOfWork
{
    public sealed class EfUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public EfUnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task ExecuteAsync(
            Func<IWriteRepositories, Task> action,
            CancellationToken cancellationToken = default)
        {
            var repositories = new EfWriteRepositories(_db);

            await action(repositories);

            await _db.SaveChangesAsync(cancellationToken);
        }

        internal sealed class EfWriteRepositories : IWriteRepositories
        {
            public IGymWriteRepository GymWriteRepository { get; }
            public IInvitationToAdministrateWriteRepository InvitationToAdministrateWriteRepository { get; }
            public IMemberWriteRepository MemberWriteRepository { get; }
            public IPaymentOrderWriteRepository PaymentOrderWriteRepository { get; }
            public ISessionWriteRepository SessionWriteRepository { get; }
            public ISubscriptionWriteRepository SubscriptionWriteRepository { get; }
            public IUserWriteRepository UserWriteRepository { get; }

            public EfWriteRepositories(ApplicationDbContext db)
            {
                GymWriteRepository = new GymWriteRepository(db);
                InvitationToAdministrateWriteRepository = new InvitationToAdministrateWriteRepository(db);
                MemberWriteRepository = new MemberWriteRepository(db);
                PaymentOrderWriteRepository = new PaymentOrderWriteRepository(db);
                SessionWriteRepository = new SessionWriteRepository(db);
                SubscriptionWriteRepository = new SubscriptionWriteRepository(db);
                UserWriteRepository = new UserWriteRepository(db);
            }
        }
    }
}
