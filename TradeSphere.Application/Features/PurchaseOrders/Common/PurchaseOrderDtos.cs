namespace TradeSphere.Application.Features.PurchaseOrders.Common;

public sealed record PurchaseOrderListItemDto(
    Guid Id, string OrderNumber, Guid SupplierId, string SupplierName,
    PurchaseOrderStatus Status, DateTimeOffset OrderDate, int LinesCount, decimal TotalAmount);

public sealed record PurchaseOrderLineDto(Guid Id, Guid ProductId, string ProductName, int Quantity, decimal UnitPrice, string Currency);

public sealed record PurchaseOrderDetailsDto(
    Guid Id, string OrderNumber, Guid SupplierId, string SupplierName,
    PurchaseOrderStatus Status, DateTimeOffset OrderDate, List<PurchaseOrderLineDto> Lines, decimal TotalAmount);