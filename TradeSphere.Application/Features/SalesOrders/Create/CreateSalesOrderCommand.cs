namespace TradeSphere.Application.Features.SalesOrders.Create;

public sealed record CreateSalesOrderCommand(Guid CustomerId, DateTimeOffset OrderDate) : IRequest<Result<Guid>>, ITransactionalRequest;