namespace TradeSphere.Application.Features.Shipments.GetList;

public sealed class GetShipmentsQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetShipmentsQuery, PaginatedList<ShipmentListItemDto>>
{
    public async Task<PaginatedList<ShipmentListItemDto>> Handle(GetShipmentsQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Shipments
            .Join(dbContext.PurchaseOrders, s => s.PurchaseOrderId, po => po.Id, (s, po) => new { Shipment = s, po.OrderNumber })
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.TrackingNumber))
            query = query.Where(x => x.Shipment.TrackingNumber.Contains(request.TrackingNumber));

        if (request.Status.HasValue)
            query = query.Where(x => x.Shipment.Status == request.Status.Value);

        if (request.PurchaseOrderId.HasValue)
            query = query.Where(x => x.Shipment.PurchaseOrderId == request.PurchaseOrderId.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.Shipment.CreatedAtUtc)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new ShipmentListItemDto(
                x.Shipment.Id, x.Shipment.PurchaseOrderId, x.OrderNumber, x.Shipment.TrackingNumber,
                x.Shipment.Carrier, x.Shipment.Status, x.Shipment.ShippedAtUtc, x.Shipment.ArrivedAtUtc))
            .ToListAsync(cancellationToken);

        return new PaginatedList<ShipmentListItemDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}