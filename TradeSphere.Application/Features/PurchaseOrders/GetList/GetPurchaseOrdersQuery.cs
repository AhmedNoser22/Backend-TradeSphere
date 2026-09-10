namespace TradeSphere.Application.Features.PurchaseOrders.GetList;

public sealed record GetPurchaseOrdersQuery(
    string? SearchTerm, PurchaseOrderStatus? Status, Guid? SupplierId,
    int PageNumber = 1, int PageSize = 20) : IRequest<PaginatedList<PurchaseOrderListItemDto>>;