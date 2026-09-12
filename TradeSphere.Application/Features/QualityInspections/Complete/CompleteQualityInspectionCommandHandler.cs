namespace TradeSphere.Application.Features.QualityInspections.Complete;

public sealed class CompleteQualityInspectionCommandHandler(IRepository<QualityInspection> inspectionRepository)
    : IRequestHandler<CompleteQualityInspectionCommand, Result>
{
    public async Task<Result> Handle(CompleteQualityInspectionCommand request, CancellationToken cancellationToken)
    {
        // Lines must be loaded — Complete() checks "at least one line exists".
        var inspection = await inspectionRepository.FirstOrDefaultAsync(
            new QualityInspectionByIdWithLinesSpecification(request.QualityInspectionId), cancellationToken)
            ?? throw new NotFoundException(nameof(QualityInspection), request.QualityInspectionId);

        try
        {
            // Raises QualityInspectionCompletedEvent internally — Warehouse's
            // event handler receives stock automatically once this saves.
            inspection.Complete();
        }
        catch (DomainException ex)
        {
            return Result.Failure(ex.Message);
        }

        inspectionRepository.Update(inspection);
        return Result.Success();
    }
}