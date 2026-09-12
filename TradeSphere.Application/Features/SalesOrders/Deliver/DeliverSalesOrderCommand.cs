namespace TradeSphere.Application.Features.SalesOrders.Deliver;

public sealed record DeliverSalesOrderCommand(Guid SalesOrderId) : IRequest<Result>, ITransactionalRequest;