namespace TradeSphere.Application.Features.Payments.EventHandlers;
public sealed class SalesOrderFulfilledEventHandler(IApplicationDbContext dbContext)
    : INotificationHandler<DomainEventNotification<SalesOrderFulfilledEvent>>
{
    public async Task Handle(DomainEventNotification<SalesOrderFulfilledEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        if (await dbContext.Payments.AnyAsync(p => p.ReferenceOrderId == domainEvent.SalesOrderId, cancellationToken))
            return;

        var payment = new Payment(PaymentDirection.Incoming, domainEvent.SalesOrderId, domainEvent.TotalAmount, Domain.Common.BusinessConstants.BaseCurrency);
        await dbContext.Payments.AddAsync(payment, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}