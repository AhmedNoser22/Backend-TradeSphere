namespace TradeSphere.Application.Features.Shipments.MarkAsArrived;

public sealed record MarkShipmentAsArrivedCommand(Guid ShipmentId) : IRequest<Result>, ITransactionalRequest;