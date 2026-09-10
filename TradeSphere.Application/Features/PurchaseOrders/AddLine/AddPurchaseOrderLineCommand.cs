namespace TradeSphere.Application.Features.PurchaseOrders.AddLine;

public sealed record AddPurchaseOrderLineCommand(
    Guid PurchaseOrderId, Guid ProductId, int Quantity, decimal UnitPrice, string Currency) : IRequest<Result>, ITransactionalRequest;