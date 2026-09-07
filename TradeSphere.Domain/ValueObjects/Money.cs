namespace TradeSphere.Domain.ValueObjects;

/// <summary>
/// A monetary amount + its currency together, so we never accidentally add
/// USD to EGP. Value Object = no Id, two Moneys with the same amount/currency
/// are considered equal (via 'record').
/// </summary>
public sealed record Money(decimal Amount, string Currency)
{
    public static Money Zero(string currency) => new(0m, currency);

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException($"Cannot add {a.Currency} to {b.Currency}.");
        return a with { Amount = a.Amount + b.Amount };
    }

    public static Money operator -(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException($"Cannot subtract {b.Currency} from {a.Currency}.");
        return a with { Amount = a.Amount - b.Amount };
    }
}