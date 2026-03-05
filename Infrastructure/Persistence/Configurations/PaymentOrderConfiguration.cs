using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PaymentOrderConfiguration
        : IEntityTypeConfiguration<PaymentOrder>
    {
        public void Configure(EntityTypeBuilder<PaymentOrder> builder)
        {
            builder.ToTable("PaymentOrders");

            builder.HasKey(paymentOrder => paymentOrder.Id);

            builder.Property(paymentOrder => paymentOrder.Id)
                .ValueGeneratedNever();

            builder.Property(paymentOrder => paymentOrder.Amount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(paymentOrder => paymentOrder.PaymentMean)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(paymentOrder => paymentOrder.IsPaid)
                .IsRequired();
        }
    }
}