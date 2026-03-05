using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Gym> Gyms => Set<Gym>();
        public DbSet<InvitationToAdministrate> InvitationsToAdministrate => Set<InvitationToAdministrate>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<PaymentOrder> PaymentOrders => Set<PaymentOrder>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();
        public DbSet<User> Users => Set<User>();

        public DbSet<Session> Sessions => Set<Session>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDbContext).Assembly
            );

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.IsOwned())
                    continue;

                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime>("CreatedAt");

                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime>("UpdatedAt");

                modelBuilder.Entity(entityType.ClrType)
                    .Property<bool>("IsDeleted")
                    .HasDefaultValue(false);

                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime?>("DeletedAt")
                    .HasDefaultValue(null);

                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(
                        GenerateIsDeletedFilter(entityType.ClrType));
            }

            base.OnModelCreating(modelBuilder);
        }

        private static LambdaExpression GenerateIsDeletedFilter(Type type)
        {
            var parameter = Expression.Parameter(type, "e");

            var propertyMethod = typeof(EF)
                .GetMethod("Property")!
                .MakeGenericMethod(typeof(bool));

            var isDeletedProperty = Expression.Call(
                propertyMethod,
                parameter,
                Expression.Constant("IsDeleted"));

            var compareExpression = Expression.Equal(
                isDeletedProperty,
                Expression.Constant(false));

            return Expression.Lambda(compareExpression, parameter);
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedAt").CurrentValue = now;
                    entry.Property("UpdatedAt").CurrentValue = now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("UpdatedAt").CurrentValue = now;
                }

                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Property("IsDeleted").CurrentValue = true;
                    entry.Property("DeletedAt").CurrentValue = now;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
