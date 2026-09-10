namespace TradeSphere.Application.Features.Shipments.MarkAsInTransit;

public sealed record MarkShipmentAsInTransitCommand(Guid ShipmentId) : IRequest<Result>, ITransactionalRequest;