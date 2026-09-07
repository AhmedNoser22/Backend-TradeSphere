namespace TradeSphere.Domain.Entities;
public sealed class Shipment : AuditableEntity
{
    public Guid PurchaseOrderId { get; private set; }
    public string TrackingNumber { get; private set; } = default!;
    public string Carrier { get; private set; } = default!;
    public ShipmentStatus Status { get; private set; } = ShipmentStatus.Preparing;
    public DateTimeOffset? ShippedAtUtc { get; private set; }
    public DateTimeOffset? ArrivedAtUtc { get; private set; }

    private Shipment() { }

    public Shipment(Guid purchaseOrderId, string trackingNumber, string carrier)
    {
        PurchaseOrderId = purchaseOrderId;
        TrackingNumber = trackingNumber;
        Carrier = carrier;
    }

    public void MarkAsShipped()
    {
        EnsureNextStep(ShipmentStatus.Preparing, ShipmentStatus.Shipped);
        Status = ShipmentStatus.Shipped;
        ShippedAtUtc = DateTimeOffset.UtcNow;
    }

    public void MarkAsInTransit()
    {
        EnsureNextStep(ShipmentStatus.Shipped, ShipmentStatus.InTransit);
        Status = ShipmentStatus.InTransit;
    }

    public void MarkAsArrived()
    {
        EnsureNextStep(ShipmentStatus.InTransit, ShipmentStatus.Arrived);
        Status = ShipmentStatus.Arrived;
        ArrivedAtUtc = DateTimeOffset.UtcNow;

        // Customs module reacts to this without Shipment knowing Customs exists.
        RaiseDomainEvent(new ShipmentArrivedEvent(Id, PurchaseOrderId));
    }

    private void EnsureNextStep(ShipmentStatus requiredCurrent, ShipmentStatus target)
    {
        if (Status != requiredCurrent)
            throw new InvalidStateTransitionException(nameof(Shipment), Status.ToString(), target.ToString());
    }
}