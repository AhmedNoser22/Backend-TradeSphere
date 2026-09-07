namespace TradeSphere.Application.Features.Auth.EventHandlers;

public sealed class UserRegisteredEventHandler(IEmailService emailService)
    : INotificationHandler<DomainEventNotification<UserRegisteredEvent>>
{
    public Task Handle(DomainEventNotification<UserRegisteredEvent> notification, CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;
        var body = $"""
            <p>Welcome {e.FullName},</p>
            <p>Your confirmation code is: <strong>{e.ConfirmationCode}</strong></p>
            <p>This code expires in 30 minutes.</p>
            """;

        return emailService.SendAsync(e.Email, "Confirm your TradeSphere account", body, cancellationToken);
    }
}