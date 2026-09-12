namespace TradeSphere.Application.Features.SalesOrders.Common;

public sealed record SalesOrderListItemDto(
    Guid Id, string OrderNumber, Guid CustomerId, string CustomerName,
    SalesOrderStatus Status, DateTimeOffset OrderDate, int LinesCount, decimal TotalAmount);

public sealed record SalesOrderLineDto(Guid Id, Guid ProductId, string ProductName, int Quantity, decimal UnitPrice);

public sealed record SalesOrderDetailsDto(
    Guid Id, string OrderNumber, Guid CustomerId, string CustomerName,
    SalesOrderStatus Status, DateTimeOffset OrderDate, List<SalesOrderLineDto> Lines, decimal TotalAmount);