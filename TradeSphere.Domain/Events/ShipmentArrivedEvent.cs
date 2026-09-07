namespace TradeSphere.Domain.Events;

// Raised when Logistics marks a shipment as "Arrived" in Egypt.
// Customs module listens to this to open a clearance case automatically.
public sealed class ShipmentArrivedEvent(Guid shipmentId, Guid purchaseOrderId) : IDomainEvent
{
    public Guid ShipmentId { get; } = shipmentId;
    public Guid PurchaseOrderId { get; } = purchaseOrderId;
    public DateTimeOffset OccurredOnUtc { get; } = DateTimeOffset.UtcNow;
}