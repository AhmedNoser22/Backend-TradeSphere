namespace TradeSphere.Application.Features.SalesOrders.GetById;

public sealed record GetSalesOrderByIdQuery(Guid SalesOrderId) : IRequest<SalesOrderDetailsDto>;