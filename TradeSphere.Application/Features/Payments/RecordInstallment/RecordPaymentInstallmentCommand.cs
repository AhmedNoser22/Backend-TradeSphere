namespace TradeSphere.Application.Features.Payments.RecordInstallment;

public sealed record RecordPaymentInstallmentCommand(Guid PaymentId, decimal Amount) : IRequest<Result>, ITransactionalRequest;