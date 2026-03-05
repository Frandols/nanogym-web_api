using Application.Abstractions.Hashing;
using Application.Abstractions.Tokens;
using Application.Abstractions.UnitOfWork;
using Domain.Entities;
using Domain.Repositories;

namespace Application.UseCases
{
    public class LoginUseCase
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IHasher _hasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public LoginUseCase(
            IUserReadRepository userReadRepository,
            IHasher hasher,
            ITokenGenerator tokenGenerator,
            IUnitOfWork unitOfWork)
        {
            _userReadRepository = userReadRepository;
            _hasher = hasher;
            _tokenGenerator = tokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> ExecuteAsync(LoginUseCaseInput input)
        {
            var user = await _userReadRepository.GetByEmailAsync(input.Email);

            if(user == null)
                throw new InvalidOperationException($"User with email {input.Email} not found.");

            var candidatePasswordHash = _hasher.Hash(input.CandidatePassword);

            if(!user.IsCorrectPassword(candidatePasswordHash))
                throw new InvalidOperationException("Incorrect password.");

            var token = _tokenGenerator.Execute();

            var tokenHash = _hasher.Hash(token);

            var createSessionInput = new CreateSessionInput(
                UserId: user.Id,
                TokenHash: tokenHash,
                IpAddress: input.IpAddress,
                UserAgent: input.UserAgent);

            var session = new Session(createSessionInput);

            await _unitOfWork.ExecuteAsync(async writeRepositories =>
            {
                await writeRepositories.SessionWriteRepository.AddAsync(session);
            });
            
            return session.Id;
        }
    }

    public sealed record LoginUseCaseInput(
        string Email,
        string CandidatePassword,
        string IpAddress,
        string UserAgent);
}
