namespace TradeSphere.Application.Features.Payments.GetList;

public sealed class GetPaymentsQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetPaymentsQuery, PaginatedList<PaymentListItemDto>>
{
    public async Task<PaginatedList<PaymentListItemDto>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
    {
        var incoming = dbContext.Payments
            .Where(p => p.Direction == PaymentDirection.Incoming)
            .Join(dbContext.SalesOrders, p => p.ReferenceOrderId, so => so.Id,
                (p, so) => new PaymentListItemDto(p.Id, p.Direction, p.ReferenceOrderId, so.OrderNumber, so.Customer.Name,
                    p.TotalDue, p.AmountPaid, p.TotalDue - p.AmountPaid, p.Currency, p.Status));

        var outgoing = dbContext.Payments
            .Where(p => p.Direction == PaymentDirection.Outgoing)
            .Join(dbContext.PurchaseOrders, p => p.ReferenceOrderId, po => po.Id,
                (p, po) => new PaymentListItemDto(p.Id, p.Direction, p.ReferenceOrderId, po.OrderNumber, po.Supplier.Name,
                    p.TotalDue, p.AmountPaid, p.TotalDue - p.AmountPaid, p.Currency, p.Status));

        var query = request.Direction switch
        {
            PaymentDirection.Incoming => incoming,
            PaymentDirection.Outgoing => outgoing,
            _ => incoming.Concat(outgoing)
        };

        if (request.Status.HasValue)
            query = query.Where(p => p.Status == request.Status.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(p => p.RemainingBalance)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<PaymentListItemDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}