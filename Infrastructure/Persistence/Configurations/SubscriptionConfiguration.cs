using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class SubscriptionConfiguration
        : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable("Subscriptions");

            builder.HasKey(subscription => subscription.Id);

            builder.Property(subscription => subscription.Id)
                .ValueGeneratedNever();

            builder.Property(subscription => subscription.MemberId)
                .IsRequired();

            builder.Property(subscription => subscription.GymId)
                .IsRequired();

            builder.Property(subscription => subscription.StartsAt)
                .IsRequired();

            builder.Property(subscription => subscription.EndsAt)
                .IsRequired();

            builder.Property(subscription => subscription.IsPaid)
                .IsRequired();

            builder.Property(subscription => subscription.IsActive)
                .IsRequired();

            builder.Property(subscription => subscription.CreatedAt)
                .IsRequired();

            builder.Property(subscription => subscription.UpdatedAt)
                .IsRequired();

            builder.HasOne<Member>()
                .WithMany()
                .HasForeignKey(subscription => subscription.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Gym>()
                .WithMany()
                .HasForeignKey(subscription => subscription.GymId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Plan>()
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(subscription => subscription.Payments)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(subscription => subscription.Payments)
                .WithOne()
                .HasForeignKey("SubscriptionId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}