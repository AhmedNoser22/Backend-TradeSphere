public sealed class EmailConfirmationCodeResentEventHandler(IEmailService emailService)
    : INotificationHandler<DomainEventNotification<EmailConfirmationCodeResentEvent>>
{
    public Task Handle(DomainEventNotification<EmailConfirmationCodeResentEvent> notification, CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;
        var body = $"""
            <p>Your new confirmation code is: <strong>{e.ConfirmationCode}</strong></p>
            <p>This code expires in 30 minutes.</p>
            """;

        return emailService.SendAsync(e.Email, "Your new TradeSphere confirmation code", body, cancellationToken);
    }
}