namespace TradeSphere.Domain.Events;
public sealed record QualityInspectionAcceptedLine(Guid ProductId, int AcceptedQuantity);
public sealed class QualityInspectionCompletedEvent(Guid inspectionId, Guid purchaseOrderId, IReadOnlyCollection<QualityInspectionAcceptedLine> acceptedLines) : IDomainEvent
{
    public Guid InspectionId { get; } = inspectionId;
    public Guid PurchaseOrderId { get; } = purchaseOrderId;
    public IReadOnlyCollection<QualityInspectionAcceptedLine> AcceptedLines { get; } = acceptedLines;
    public DateTimeOffset OccurredOnUtc { get; } = DateTimeOffset.UtcNow;
}