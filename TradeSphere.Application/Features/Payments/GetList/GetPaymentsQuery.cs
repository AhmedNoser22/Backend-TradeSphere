namespace TradeSphere.Application.Features.Payments.GetList;
public sealed record GetPaymentsQuery(
    PaymentDirection? Direction, PaymentStatus? Status, int PageNumber = 1, int PageSize = 20)
    : IRequest<PaginatedList<PaymentListItemDto>>;