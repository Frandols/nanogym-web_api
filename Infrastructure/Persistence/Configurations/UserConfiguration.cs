using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(user => user.Id);

            builder.Property(user => user.Id)
                .ValueGeneratedNever();

            builder.Property(user => user.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(user => user.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(user => user.HasIntegration)
                .IsRequired();

            builder.Property(user => user.CreatedAt)
                .IsRequired();

            builder.Property(user => user.UpdatedAt)
                .IsRequired();

            builder.HasIndex(user => user.Email)
                .IsUnique();
        }
    }
}