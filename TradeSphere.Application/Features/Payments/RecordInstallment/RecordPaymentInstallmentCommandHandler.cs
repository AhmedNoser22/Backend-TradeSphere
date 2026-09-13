namespace TradeSphere.Application.Features.Payments.RecordInstallment;

public sealed class RecordPaymentInstallmentCommandHandler(IRepository<Payment> paymentRepository) : IRequestHandler<RecordPaymentInstallmentCommand, Result>
{
    public async Task<Result> Handle(RecordPaymentInstallmentCommand request, CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.GetByIdAsync(request.PaymentId, cancellationToken)
            ?? throw new NotFoundException(nameof(Payment), request.PaymentId);

        try
        {
            // Payment has no child entities — no explicit tracking needed
            // here, unlike the Line/Movement/Token cases fixed earlier.
            payment.RecordInstallment(request.Amount);
        }
        catch (BusinessRuleViolationException ex)
        {
            return Result.Failure(ex.Message);
        }

        paymentRepository.Update(payment);
        return Result.Success();
    }
}