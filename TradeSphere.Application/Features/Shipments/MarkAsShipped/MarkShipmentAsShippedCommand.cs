namespace TradeSphere.Application.Features.Shipments.MarkAsShipped;

public sealed record MarkShipmentAsShippedCommand(Guid ShipmentId) : IRequest<Result>, ITransactionalRequest;