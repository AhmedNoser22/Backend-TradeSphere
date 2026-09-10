namespace TradeSphere.Application.Features.Products.Common;
public sealed record ProductDto(
    Guid Id,
    string Sku,
    string Name,
    string? Description,
    string Unit,
    bool IsActive,
    DateTimeOffset CreatedAtUtc);