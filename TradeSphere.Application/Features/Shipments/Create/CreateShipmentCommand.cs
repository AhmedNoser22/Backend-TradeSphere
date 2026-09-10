namespace TradeSphere.Application.Features.Shipments.Create;

public sealed record CreateShipmentCommand(Guid PurchaseOrderId, string TrackingNumber, string Carrier) : IRequest<Result<Guid>>, ITransactionalRequest;