using Domain.Enums;

namespace Application.Abstractions.Billing
{
    public abstract class PaymentOrderBill
    {
        public Guid PaymentOrderId { get; }

        protected PaymentOrderBill(CreatePaymentOrderBillInput input)
        {
            PaymentOrderId = input.PaymentOrderId;
        }
    }

    public sealed record CreatePaymentOrderBillInput(
        Guid PaymentOrderId);

    public sealed class CashPaidPaymentOrderBill : PaymentOrderBill
    {
        public decimal Amount { get; }

        public CashPaidPaymentOrderBill(CreateCashPaidOrderBillInput input)
            : base(new CreatePaymentOrderBillInput(
                PaymentOrderId: input.PaymentOrderId))
        {
            Amount = input.Amount;
        }
    }

    public sealed record CreateCashPaidOrderBillInput(
        Guid PaymentOrderId,
        decimal Amount);

    public sealed class QRCodePaidPaymentOrderBill : PaymentOrderBill
    {
        public string QrCode { get; }

        public QRCodePaidPaymentOrderBill(CreateQRCodePaidOrderBillInput input)
            : base(new CreatePaymentOrderBillInput(
                PaymentOrderId: input.PaymentOrderId))
        {
            QrCode = input.QrCode;
        }
    }

    public sealed record CreateQRCodePaidOrderBillInput(
        Guid PaymentOrderId,
        string QrCode);
}
