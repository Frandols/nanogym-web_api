using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class InvitationToAdministrateConfiguration
        : IEntityTypeConfiguration<InvitationToAdministrate>
    {
        public void Configure(EntityTypeBuilder<InvitationToAdministrate> builder)
        {
            builder.ToTable("InvitationsToAdministrate");

            builder.HasKey(invitationToAdministrate => invitationToAdministrate.Id);

            builder.Property(invitationToAdministrate => invitationToAdministrate.Id)
                .ValueGeneratedNever();

            builder.Property(invitationToAdministrate => invitationToAdministrate.GymId)
                .IsRequired();

            builder.Property(invitationToAdministrate => invitationToAdministrate.IsUsed)
                .IsRequired();

            builder.Property(invitationToAdministrate => invitationToAdministrate.CreatedAt)
                .IsRequired();

            builder.Property(invitationToAdministrate => invitationToAdministrate.ValidFor)
                .IsRequired()
                .HasConversion(
                    v => v.Ticks,
                    v => TimeSpan.FromTicks(v)
                );

            builder.HasOne<Gym>()
                .WithMany()
                .HasForeignKey(invitationToAdministrate => invitationToAdministrate.GymId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}