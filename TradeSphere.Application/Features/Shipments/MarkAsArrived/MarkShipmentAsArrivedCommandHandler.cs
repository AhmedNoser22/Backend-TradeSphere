namespace TradeSphere.Application.Features.Shipments.MarkAsArrived;

public sealed class MarkShipmentAsArrivedCommandHandler(IRepository<Shipment> shipmentRepository) : IRequestHandler<MarkShipmentAsArrivedCommand, Result>
{
    public async Task<Result> Handle(MarkShipmentAsArrivedCommand request, CancellationToken cancellationToken)
    {
        var shipment = await shipmentRepository.GetByIdAsync(request.ShipmentId, cancellationToken)
            ?? throw new NotFoundException(nameof(Shipment), request.ShipmentId);

        try
        {
            shipment.MarkAsArrived();
        }
        catch (DomainException ex)
        {
            return Result.Failure(ex.Message);
        }

        shipmentRepository.Update(shipment);
        return Result.Success();
    }
}