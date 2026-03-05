using Domain.Repositories;

namespace Application.Abstractions.UnitOfWork
{
    public interface IWriteRepositories
    {
        IGymWriteRepository GymWriteRepository { get; }
        IInvitationToAdministrateWriteRepository InvitationToAdministrateWriteRepository { get; }
        IMemberWriteRepository MemberWriteRepository { get; }
        IPaymentOrderWriteRepository PaymentOrderWriteRepository { get; }
        ISessionWriteRepository SessionWriteRepository { get; }
        ISubscriptionWriteRepository SubscriptionWriteRepository { get; }
        IUserWriteRepository UserWriteRepository { get; }
    }

    public interface IUnitOfWork
    {
        Task ExecuteAsync(
            Func<IWriteRepositories, Task> action,
            CancellationToken cancellationToken = default);
    }
}
