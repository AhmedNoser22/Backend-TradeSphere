public sealed class PasswordResetRequestedEventHandler(IEmailService emailService)
    : INotificationHandler<DomainEventNotification<PasswordResetRequestedEvent>>
{
    public Task Handle(DomainEventNotification<PasswordResetRequestedEvent> notification, CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;
        var body = $"""
            <p>Use this code to reset your password: <strong>{e.ResetCode}</strong></p>
            <p>This code expires in 15 minutes. If you didn't request this, ignore this email.</p>
            """;

        return emailService.SendAsync(e.Email, "Reset your TradeSphere password", body, cancellationToken);
    }
}