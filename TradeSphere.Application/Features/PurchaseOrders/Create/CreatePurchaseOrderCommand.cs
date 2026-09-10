namespace TradeSphere.Application.Features.PurchaseOrders.Create;

public sealed record CreatePurchaseOrderCommand(Guid SupplierId, DateTimeOffset OrderDate) : IRequest<Result<Guid>>, ITransactionalRequest;