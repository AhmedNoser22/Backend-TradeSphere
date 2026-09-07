namespace TradeSphere.Domain.Events;
public interface IDomainEvent
{
    DateTimeOffset OccurredOnUtc { get; }
}