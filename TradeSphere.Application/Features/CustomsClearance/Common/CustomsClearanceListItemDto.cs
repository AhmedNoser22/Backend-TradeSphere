namespace TradeSphere.Application.Features.CustomsClearance.Common;

public sealed record CustomsClearanceListItemDto(
    Guid Id, Guid ShipmentId, string TrackingNumber, string? DeclarationNumber, string? Port,
    decimal DeclaredGoodsValue, decimal CustomsDuties, decimal ClearanceFees, decimal TotalLandedCost,
    CustomsClearanceStatus Status);