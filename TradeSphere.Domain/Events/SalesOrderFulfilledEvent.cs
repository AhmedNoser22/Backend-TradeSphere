namespace TradeSphere.Domain.Events;

// Raised after a sales order is delivered and stock has been deducted.
// Finance module listens to this to create the outstanding invoice/payment record.
public sealed class SalesOrderFulfilledEvent(Guid salesOrderId, Guid customerId, decimal totalAmount) : IDomainEvent
{
    public Guid SalesOrderId { get; } = salesOrderId;
    public Guid CustomerId { get; } = customerId;
    public decimal TotalAmount { get; } = totalAmount;
    public DateTimeOffset OccurredOnUtc { get; } = DateTimeOffset.UtcNow;
}