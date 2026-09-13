namespace TradeSphere.Application.Features.Payments.EventHandlers;
public sealed class PurchaseOrderConfirmedEventHandler(IApplicationDbContext dbContext)
    : INotificationHandler<DomainEventNotification<PurchaseOrderConfirmedEvent>>
{
    public async Task Handle(DomainEventNotification<PurchaseOrderConfirmedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        if (await dbContext.Payments.AnyAsync(p => p.ReferenceOrderId == domainEvent.PurchaseOrderId, cancellationToken))
            return;

        var payment = new Payment(PaymentDirection.Outgoing, domainEvent.PurchaseOrderId, domainEvent.TotalAmount, domainEvent.Currency);
        await dbContext.Payments.AddAsync(payment, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}