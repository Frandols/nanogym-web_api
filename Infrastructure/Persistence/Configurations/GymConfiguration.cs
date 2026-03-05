using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class GymConfiguration 
        : IEntityTypeConfiguration<Gym>
    {
        public void Configure(EntityTypeBuilder<Gym> builder)
        {
            builder.ToTable("Gyms");

            builder.HasKey(gym => gym.Id);

            builder.Property(gym => gym.Id)
                .ValueGeneratedNever();

            builder.Property(gym => gym.OwnerId)
                .IsRequired();

            builder.OwnsMany(g => g.Administrators, navigationBuilder =>
            {
                navigationBuilder.ToTable("Administrators");

                navigationBuilder.WithOwner()
                     .HasForeignKey("GymId");

                navigationBuilder.Property(administrator => administrator.UserId)
                     .IsRequired();

                navigationBuilder.Property(administrator => administrator.CreatedAt)
                     .IsRequired();

                navigationBuilder.HasKey("GymId", nameof(Administrator.UserId));

                navigationBuilder.HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Navigation(g => g.Administrators)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsMany(g => g.Plans, plan =>
            {
                plan.ToTable("Plans");

                plan.WithOwner().HasForeignKey("GymId");

                plan.HasKey(p => p.Id);

                plan.Property(p => p.Id)
                    .ValueGeneratedNever();

                plan.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                plan.Property(p => p.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                plan.Property(p => p.IsActive)
                    .IsRequired();

                plan.Property(p => p.CreatedAt)
                    .IsRequired();

                plan.Property(p => p.UpdatedAt)
                    .IsRequired();

                plan.OwnsOne(p => p.Duration, duration =>
                {
                    duration.Property(d => d.Magnitude)
                        .HasColumnName("DurationMagnitude")
                        .IsRequired();

                    duration.Property(d => d.Unit)
                        .HasColumnName("DurationUnit")
                        .IsRequired();
                });
            });

            builder.Navigation(g => g.Plans)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(gym => gym.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.OwnsOne(gym => gym.Location, location =>
            {
                location.Property(l => l.Latitude)
                    .HasColumnName("LocationLatitude")
                    .IsRequired();

                location.Property(l => l.Longitude)
                    .HasColumnName("LocationLongitude")
                    .IsRequired();

                location.Property(l => l.Address)
                    .HasColumnName("LocationAddress")
                    .IsRequired();
            });

            builder.Property(gym => gym.IsActive)
                .IsRequired();

            builder.Property(gym => gym.CreatedAt)
                .IsRequired();

            builder.Property(gym => gym.UpdatedAt)
                .IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
