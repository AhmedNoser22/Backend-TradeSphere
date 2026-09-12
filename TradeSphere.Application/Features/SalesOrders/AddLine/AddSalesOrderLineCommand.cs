namespace TradeSphere.Application.Features.SalesOrders.AddLine;

public sealed record AddSalesOrderLineCommand(Guid SalesOrderId, Guid ProductId, int Quantity, decimal UnitPrice) : IRequest<Result>, ITransactionalRequest;