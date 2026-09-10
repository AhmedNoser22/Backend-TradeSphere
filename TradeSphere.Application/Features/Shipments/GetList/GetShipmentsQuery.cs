namespace TradeSphere.Application.Features.Shipments.GetList;
public sealed record GetShipmentsQuery(
    string? TrackingNumber, ShipmentStatus? Status, Guid? PurchaseOrderId,
    int PageNumber = 1, int PageSize = 20) : IRequest<PaginatedList<ShipmentListItemDto>>;