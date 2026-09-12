namespace TradeSphere.Application.Features.QualityInspections.Create;

public sealed class CreateQualityInspectionCommandHandler(
    IRepository<Domain.Entities.CustomsClearance> customsRepository,
    IRepository<Shipment> shipmentRepository,
    IRepository<QualityInspection> inspectionRepository) : IRequestHandler<CreateQualityInspectionCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateQualityInspectionCommand request, CancellationToken cancellationToken)
    {
        var clearance = await customsRepository.GetByIdAsync(request.CustomsClearanceId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.CustomsClearance), request.CustomsClearanceId);

        if (clearance.Status != CustomsClearanceStatus.Cleared)
            return Result<Guid>.Failure("Inspection can only start after customs clearance is Cleared.");

        if (await inspectionRepository.AnyAsync(new QualityInspectionByCustomsClearanceIdSpecification(request.CustomsClearanceId), cancellationToken))
            return Result<Guid>.Failure("An inspection already exists for this customs clearance.");

        // CustomsClearance only stores ShipmentId, not PurchaseOrderId directly —
        // we need the Shipment to trace back to the order being inspected.
        var shipment = await shipmentRepository.GetByIdAsync(clearance.ShipmentId, cancellationToken)
            ?? throw new NotFoundException(nameof(Shipment), clearance.ShipmentId);

        var inspection = new QualityInspection(clearance.Id, shipment.PurchaseOrderId);
        await inspectionRepository.AddAsync(inspection, cancellationToken);

        return Result<Guid>.Success(inspection.Id);
    }
}