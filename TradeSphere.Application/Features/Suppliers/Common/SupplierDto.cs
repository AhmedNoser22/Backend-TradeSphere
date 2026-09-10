namespace TradeSphere.Application.Features.Suppliers.Common;

public sealed record SupplierDto(
    Guid Id,
    string Name,
    string Country,
    string Email,
    string? Phone,
    bool IsActive,
    DateTimeOffset CreatedAtUtc);