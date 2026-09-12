namespace TradeSphere.Application.Features.SalesOrders.GetList;
public sealed record GetSalesOrdersQuery(
    string? SearchTerm, SalesOrderStatus? Status, Guid? CustomerId,
    int PageNumber = 1, int PageSize = 20) : IRequest<PaginatedList<SalesOrderListItemDto>>;