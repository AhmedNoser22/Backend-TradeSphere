public sealed class PasswordResetRequestedEventHandler(IEmailService emailService) : INotificationHandler<DomainEventNotification<PasswordResetRequestedEvent>>
{
    public Task Handle(DomainEventNotification<PasswordResetRequestedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var body = $"""
            <p>Use this code to reset your password: <strong>{domainEvent.ResetCode}</strong></p>
            <p>This code expires in 15 minutes. If you didn't request this, ignore this email.</p>
            """;

        return emailService.SendAsync(domainEvent.Email, "Reset your TradeSphere password", body, cancellationToken);
    }
}