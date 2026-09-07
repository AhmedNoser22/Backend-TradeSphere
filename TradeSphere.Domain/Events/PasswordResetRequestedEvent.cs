namespace TradeSphere.Domain.Events;

public sealed class PasswordResetRequestedEvent(Guid userId, string email, string resetCode) : IDomainEvent
{
    public Guid UserId { get; } = userId;
    public string Email { get; } = email;
    public string ResetCode { get; } = resetCode;
    public DateTimeOffset OccurredOnUtc { get; } = DateTimeOffset.UtcNow;
}