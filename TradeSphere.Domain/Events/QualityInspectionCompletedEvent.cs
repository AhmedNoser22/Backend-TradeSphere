namespace TradeSphere.Domain.Events;

// Raised when QC finishes inspecting a shipment's goods.
// Warehouse module listens to this to know how much stock it may receive.
public sealed class QualityInspectionCompletedEvent(Guid inspectionId, Guid purchaseOrderId, int acceptedQuantity, int rejectedQuantity, int missingQuantity) : IDomainEvent
{
    public Guid InspectionId { get; } = inspectionId;
    public Guid PurchaseOrderId { get; } = purchaseOrderId;
    public int AcceptedQuantity { get; } = acceptedQuantity;
    public int RejectedQuantity { get; } = rejectedQuantity;
    public int MissingQuantity { get; } = missingQuantity;
    public DateTimeOffset OccurredOnUtc { get; } = DateTimeOffset.UtcNow;
}