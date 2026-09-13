namespace TradeSphere.Application.Features.Payments.RecordInstallment;

public sealed class RecordPaymentInstallmentCommandValidator : AbstractValidator<RecordPaymentInstallmentCommand>
{
    public RecordPaymentInstallmentCommandValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}