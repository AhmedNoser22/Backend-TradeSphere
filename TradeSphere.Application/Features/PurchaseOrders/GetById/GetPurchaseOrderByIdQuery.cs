namespace TradeSphere.Application.Features.PurchaseOrders.GetById;

public sealed record GetPurchaseOrderByIdQuery(Guid PurchaseOrderId) : IRequest<PurchaseOrderDetailsDto>;