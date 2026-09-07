namespace TradeSphere.Domain.ValueObjects;

// Used for both Supplier and Customer addresses. Plain data, no identity of its own.
public sealed record Address(string Country, string City, string Street, string? PostalCode);