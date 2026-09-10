namespace TradeSphere.Application.Features.PurchaseOrders.Cancel;

public sealed record CancelPurchaseOrderCommand(Guid PurchaseOrderId) : IRequest<Result>, ITransactionalRequest;