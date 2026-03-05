using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public sealed class SessionConfiguration
        : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("Sessions");

            builder.HasKey(session => session.Id);

            builder.Property(session => session.Id)
                   .ValueGeneratedNever();

            builder.Property(session => session.UserId)
                   .IsRequired();

            builder.Property(session => session.TokenHash)
                    .IsRequired();
                
            builder.Property(session => session.IpAddress)
                   .IsRequired()
                   .HasMaxLength(45);

            builder.Property(session => session.UserAgent)
                   .IsRequired()
                   .HasMaxLength(512);

            builder.Property(session => session.IsClosed)
                   .IsRequired();

            builder.Property(session => session.CreatedAt)
                   .IsRequired();
        }
    }
}