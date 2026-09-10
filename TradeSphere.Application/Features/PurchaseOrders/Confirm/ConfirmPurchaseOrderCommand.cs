namespace TradeSphere.Application.Features.PurchaseOrders.Confirm;

public sealed record ConfirmPurchaseOrderCommand(Guid PurchaseOrderId) : IRequest<Result>, ITransactionalRequest;