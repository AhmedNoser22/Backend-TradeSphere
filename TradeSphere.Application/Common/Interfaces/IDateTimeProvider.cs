namespace TradeSphere.Application.Common.Interfaces;
public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}