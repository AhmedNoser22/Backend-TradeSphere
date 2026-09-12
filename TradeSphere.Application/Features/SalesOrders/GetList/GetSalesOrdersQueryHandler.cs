namespace TradeSphere.Application.Features.SalesOrders.GetList;

public sealed class GetSalesOrdersQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetSalesOrdersQuery, PaginatedList<SalesOrderListItemDto>>
{
    public async Task<PaginatedList<SalesOrderListItemDto>> Handle(GetSalesOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.SalesOrders.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(so => so.OrderNumber.Contains(request.SearchTerm));

        if (request.Status.HasValue)
            query = query.Where(so => so.Status == request.Status.Value);

        if (request.CustomerId.HasValue)
            query = query.Where(so => so.CustomerId == request.CustomerId.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(so => so.OrderDate)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(so => new SalesOrderListItemDto(
                so.Id, so.OrderNumber, so.CustomerId, so.Customer.Name, so.Status, so.OrderDate,
                so.Lines.Count, so.Lines.Sum(l => l.Quantity * l.UnitPrice)))
            .ToListAsync(cancellationToken);

        return new PaginatedList<SalesOrderListItemDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}