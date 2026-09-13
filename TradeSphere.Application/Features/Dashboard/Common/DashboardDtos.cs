namespace TradeSphere.Application.Features.Dashboard.Common;

public sealed record DelayedCustomsClearanceDto(Guid Id, Guid ShipmentId, CustomsClearanceStatus Status, DateTimeOffset OpenedAtUtc, int DaysOpen);

public sealed record DashboardSummaryDto(
    decimal TotalSalesAmount,
    decimal TotalPurchasesAmount,
    decimal TotalInventoryValue,
    decimal TotalReceivables,
    decimal TotalPayables,
    List<InventoryStockListItemDto> LowStockProducts,
    List<DelayedCustomsClearanceDto> DelayedCustomsClearances);