namespace TradeSphere.Application.Features.Shipments.MarkAsInTransit;

public sealed class MarkShipmentAsInTransitCommandHandler(IRepository<Shipment> shipmentRepository) : IRequestHandler<MarkShipmentAsInTransitCommand, Result>
{
    public async Task<Result> Handle(MarkShipmentAsInTransitCommand request, CancellationToken cancellationToken)
    {
        var shipment = await shipmentRepository.GetByIdAsync(request.ShipmentId, cancellationToken)
            ?? throw new NotFoundException(nameof(Shipment), request.ShipmentId);

        try
        {
            shipment.MarkAsInTransit();
        }
        catch (DomainException ex)
        {
            return Result.Failure(ex.Message);
        }

        shipmentRepository.Update(shipment);
        return Result.Success();
    }
}