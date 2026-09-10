namespace TradeSphere.Application.Features.Shipments.Create;

public sealed class CreateShipmentCommandHandler(
    IRepository<PurchaseOrder> purchaseOrderRepository,
    IRepository<Shipment> shipmentRepository) : IRequestHandler<CreateShipmentCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateShipmentCommand request, CancellationToken cancellationToken)
    {
        var purchaseOrder = await purchaseOrderRepository.GetByIdAsync(request.PurchaseOrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(PurchaseOrder), request.PurchaseOrderId);
        try
        {
            purchaseOrder.MarkPartiallyShipped();
        }
        catch (DomainException ex)
        {
            return Result<Guid>.Failure(ex.Message);
        }

        var shipment = new Shipment(request.PurchaseOrderId, request.TrackingNumber, request.Carrier);
        await shipmentRepository.AddAsync(shipment, cancellationToken);
        purchaseOrderRepository.Update(purchaseOrder);

        return Result<Guid>.Success(shipment.Id);
    }
}