namespace TradeSphere.Domain.Events;
public sealed class UserRegisteredEvent(Guid userId, string email, string fullName, string confirmationCode) : IDomainEvent
{
    public Guid UserId { get; } = userId;
    public string Email { get; } = email;
    public string FullName { get; } = fullName;
    public string ConfirmationCode { get; } = confirmationCode;
    public DateTimeOffset OccurredOnUtc { get; } = DateTimeOffset.UtcNow;
}