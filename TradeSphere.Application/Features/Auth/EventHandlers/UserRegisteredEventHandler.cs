namespace TradeSphere.Application.Features.Auth.EventHandlers;

public sealed class UserRegisteredEventHandler(IEmailService emailService) : INotificationHandler<DomainEventNotification<UserRegisteredEvent>>
{
    public Task Handle(DomainEventNotification<UserRegisteredEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var body = $"""
            <p>Welcome {domainEvent.FullName},</p>
            <p>Your confirmation code is: <strong>{domainEvent.ConfirmationCode}</strong></p>
            <p>This code expires in 30 minutes.</p>
            """;

        return emailService.SendAsync(domainEvent.Email, "Confirm your TradeSphere account", body, cancellationToken);
    }
}