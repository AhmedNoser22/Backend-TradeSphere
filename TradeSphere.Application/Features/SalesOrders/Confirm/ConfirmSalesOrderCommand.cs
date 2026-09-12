namespace TradeSphere.Application.Features.SalesOrders.Confirm;

public sealed record ConfirmSalesOrderCommand(Guid SalesOrderId) : IRequest<Result>, ITransactionalRequest;