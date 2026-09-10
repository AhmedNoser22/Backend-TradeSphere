namespace TradeSphere.Application.Features.Shipments.MarkAsShipped;

public sealed class MarkShipmentAsShippedCommandHandler(IRepository<Shipment> shipmentRepository) : IRequestHandler<MarkShipmentAsShippedCommand, Result>
{
    public async Task<Result> Handle(MarkShipmentAsShippedCommand request, CancellationToken cancellationToken)
    {
        var shipment = await shipmentRepository.GetByIdAsync(request.ShipmentId, cancellationToken)
            ?? throw new NotFoundException(nameof(Shipment), request.ShipmentId);

        try
        {
            shipment.MarkAsShipped();
        }
        catch (DomainException ex)
        {
            return Result.Failure(ex.Message);
        }

        shipmentRepository.Update(shipment);
        return Result.Success();
    }
}