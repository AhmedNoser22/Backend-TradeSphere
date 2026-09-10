namespace TradeSphere.Application.Features.PurchaseOrders.GetList;

public sealed class GetPurchaseOrdersQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetPurchaseOrdersQuery, PaginatedList<PurchaseOrderListItemDto>>
{
    public async Task<PaginatedList<PurchaseOrderListItemDto>> Handle(GetPurchaseOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.PurchaseOrders.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(po => po.OrderNumber.Contains(request.SearchTerm));

        if (request.Status.HasValue)
            query = query.Where(po => po.Status == request.Status.Value);

        if (request.SupplierId.HasValue)
            query = query.Where(po => po.SupplierId == request.SupplierId.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(po => po.OrderDate)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(po => new PurchaseOrderListItemDto(
                po.Id, po.OrderNumber, po.SupplierId, po.Supplier.Name, po.Status, po.OrderDate,
                po.Lines.Count, po.Lines.Sum(l => l.Quantity * l.UnitPrice)))
            .ToListAsync(cancellationToken);

        return new PaginatedList<PurchaseOrderListItemDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}