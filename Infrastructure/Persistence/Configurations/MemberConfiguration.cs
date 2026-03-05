using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("Members");

            builder.HasKey(member => member.Id);

            builder.Property(member => member.Id)
                .ValueGeneratedNever();

            builder.Property(member => member.GymId)
                .IsRequired();

            builder.Property(member => member.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(member => member.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(member => member.CreatedAt)
                .IsRequired();

            builder.Property(member => member.UpdatedAt)
                .IsRequired();

            builder.HasOne<Gym>()
                .WithMany()
                .HasForeignKey(member => member.GymId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}