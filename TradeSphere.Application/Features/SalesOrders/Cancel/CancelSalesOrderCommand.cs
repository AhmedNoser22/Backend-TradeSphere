namespace TradeSphere.Application.Features.SalesOrders.Cancel;

public sealed record CancelSalesOrderCommand(Guid SalesOrderId) : IRequest<Result>, ITransactionalRequest;