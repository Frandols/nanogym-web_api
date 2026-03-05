using Application.Abstractions.Hashing;
using Domain.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace WebApi.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddSessionAuthentication(
            this IServiceCollection services)
        {
            services.AddAuthentication("SessionScheme")
                .AddScheme<AuthenticationSchemeOptions, SessionAuthenticationHandler>(
                    "SessionScheme",
                    options => { });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsAuthenticated", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddAuthenticationSchemes("SessionScheme");
                });
            });

            return services;
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetId(this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue("UserId");

            if (value is null)
                throw new UnauthorizedAccessException("User ID not found.");

            return Guid.Parse(value);
        }

        public static Guid GetSessionId(this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue("SessionId");

            if (value is null)
                throw new UnauthorizedAccessException("Session ID not found.");

            return Guid.Parse(value);
        }
    }

    public sealed class SessionAuthenticationHandler
    : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ISessionReadRepository _sessionReadRepository;
        private readonly IHasher _hasher;

        public SessionAuthenticationHandler(
            ISessionReadRepository sessionReadRepository,
            IHasher hasher,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
            _sessionReadRepository = sessionReadRepository;
            _hasher = hasher;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(token))
                return AuthenticateResult.NoResult();

            var tokenHash = _hasher.Hash(token);

            var session = await _sessionReadRepository.GetByTokenHashAsync(tokenHash);

            if (session is null || session.IsClosed)
                return AuthenticateResult.Fail("Invalid session.");

            var claims = new[]
            {
                new Claim("UserId", session.UserId.ToString()),
                new Claim("SessionId", session.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
