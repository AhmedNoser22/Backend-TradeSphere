namespace TradeSphere.Application.Features.Payments.GetById;

public sealed record GetPaymentByIdQuery(Guid PaymentId) : IRequest<PaymentListItemDto>;