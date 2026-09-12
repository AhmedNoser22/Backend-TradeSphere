namespace TradeSphere.Application.Features.Customers.Common;

public sealed record CustomerListItemDto(Guid Id, string Name, string Email, string? Phone, bool IsActive);

public sealed record CustomerDetailsDto(
    Guid Id, string Name, string Email, string? Phone,
    string? Country, string? City, string? Street, string? PostalCode, bool IsActive);