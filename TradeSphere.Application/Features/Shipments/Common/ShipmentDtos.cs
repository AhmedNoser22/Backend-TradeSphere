namespace TradeSphere.Application.Features.Shipments.Common;

public sealed record ShipmentListItemDto(
    Guid Id, Guid PurchaseOrderId, string PurchaseOrderNumber, string TrackingNumber, string Carrier,
    ShipmentStatus Status, DateTimeOffset? ShippedAtUtc, DateTimeOffset? ArrivedAtUtc);