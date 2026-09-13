namespace TradeSphere.Domain.Events;

public sealed class PurchaseOrderConfirmedEvent(Guid purchaseOrderId, decimal totalAmount, string currency) : IDomainEvent
{
    public Guid PurchaseOrderId { get; } = purchaseOrderId;
    public decimal TotalAmount { get; } = totalAmount;
    public string Currency { get; } = currency;
    public DateTimeOffset OccurredOnUtc { get; } = DateTimeOffset.UtcNow;
}