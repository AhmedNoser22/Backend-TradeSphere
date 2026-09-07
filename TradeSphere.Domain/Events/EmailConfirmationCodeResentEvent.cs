namespace TradeSphere.Domain.Events;

public sealed class EmailConfirmationCodeResentEvent(Guid userId, string email, string confirmationCode) : IDomainEvent
{
    public Guid UserId { get; } = userId;
    public string Email { get; } = email;
    public string ConfirmationCode { get; } = confirmationCode;
    public DateTimeOffset OccurredOnUtc { get; } = DateTimeOffset.UtcNow;
}