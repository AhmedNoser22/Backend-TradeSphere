namespace TradeSphere.Application.Features.CustomsClearance.EventHandlers;

public sealed class ShipmentArrivedEventHandler(IApplicationDbContext dbContext)
    : INotificationHandler<DomainEventNotification<ShipmentArrivedEvent>>
{
    public async Task Handle(DomainEventNotification<ShipmentArrivedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var clearance = new Domain.Entities.CustomsClearance(domainEvent.ShipmentId);
        await dbContext.CustomsClearances.AddAsync(clearance, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}