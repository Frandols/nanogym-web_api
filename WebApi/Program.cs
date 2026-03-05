using Application.Abstractions.Hashing;
using Application.Abstractions.Tokens;
using Application.Abstractions.UnitOfWork;
using Application.UseCases;
using Domain.Repositories;
using Infrastructure.Hashing;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Tokens;
using Infrastructure.UnitOfWork;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// <--- BUILDER CUSTOM CONFIGURATION STARTS --->


/* 1.   REPOSITORIES             */
/*-------------------------------*/
/* 1.1. Gym                      */ builder.Services.AddScoped<IGymReadRepository, GymReadRepository>();
/* 1.2. InvitationToAdministrate */ builder.Services.AddScoped<IInvitationToAdministrateReadRepository, InvitationToAdministrateReadRepository>();
/* 1.3. Member                   */ builder.Services.AddScoped<IMemberReadRepository, MemberReadRepository>();
/* 1.4. PaymentOrder             */ builder.Services.AddScoped<IPaymentOrderReadRepository, PaymentOrderReadRepository>();
/* 1.5. Session                  */ builder.Services.AddScoped<ISessionReadRepository, SessionReadRepository>();
/* 1.6. Subscription             */ builder.Services.AddScoped<ISubscriptionReadRepository, SubscriptionReadRepository>();
/* 1.7. User                     */ builder.Services.AddScoped<IUserReadRepository, UserReadRepository>();
/*-------------------------------*/


/* 2.   HASHING                  */
/*-------------------------------*/
/* *.   Hasher                   */ builder.Services.AddScoped<IHasher, IdentityHasher>();
/*-------------------------------*/


/* 3.   TOKENS                   */
/*-------------------------------*/
/* *.   TokenGenerator           */ builder.Services.AddScoped<ITokenGenerator, CryptographyTokenGenerator>();
/*-------------------------------*/


/* 4.   UNIT OF WORK             */
/*-------------------------------*/
/* *.   UnitOfWork               */ builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
/*-------------------------------*/


/* 4.   USE CASES                */
/*-------------------------------*/
/* 4.1. CreateGym                */ builder.Services.AddScoped<CreateGymUseCase>();
/* 4.2. CreateMember             */ builder.Services.AddScoped<CreateMemberUseCase>();
/* 4.3. CreatePayment            */ builder.Services.AddScoped<CreatePaymentOrderUseCase>();
/* 4.4. CreateSubscription       */ builder.Services.AddScoped<CreateSubscriptionUseCase>();
/* 4.5. CreateUser               */ builder.Services.AddScoped<RegisterUseCase>();
/* 4.6. GeneratePaymentOrderBill */ builder.Services.AddScoped<GeneratePaymentOrderBillUseCase>();
/* 4.7. InviteAdministrator      */ builder.Services.AddScoped<InviteAdministratorUseCase>();
/* 4.8. CreateGym                */ builder.Services.AddScoped<UseInvitationToAdministrateUseCase>();
/*-------------------------------*/


/* 5.   EXTENSIONS               */
/*-------------------------------*/
/* 1.   Authentication           */ builder.Services.AddSessionAuthentication();
/*-------------------------------*/


// <---  BUILDER CUSTOM CONFIGURATION ENDS  --->

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
