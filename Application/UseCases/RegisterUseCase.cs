using Application.Abstractions.Hashing;
using Application.Abstractions.UnitOfWork;
using Domain.Entities;
namespace Application.UseCases
{
    public class RegisterUseCase
    {
        private readonly IHasher _hasher;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUseCase(
            IHasher hasher,
            IUnitOfWork unitOfWork)
        {
            _hasher = hasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> ExecuteAsync(RegisterUseCaseInput input)
        {
            var passwordHash = _hasher.Hash(input.Password);

            var createUserInput = new CreateUserInput(
                Name: input.Name,
                Email: input.Email,
                PasswordHash: passwordHash);

            var user = new User(createUserInput);

            await _unitOfWork.ExecuteAsync(async writeRepositories =>
            {
                await writeRepositories.UserWriteRepository.AddAsync(user);
            });

            return user.Id;
        }
    }

    public sealed record RegisterUseCaseInput(
        string Name,
        string Email,
        string Password);
}
