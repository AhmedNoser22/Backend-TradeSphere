namespace TradeSphere.Domain.ValueObjects;

// Email + phone bundled together since almost every party (supplier, customer, user)
// needs both, and it keeps the owning entity's property list shorter.
public sealed record ContactInfo(string Email, string? Phone);