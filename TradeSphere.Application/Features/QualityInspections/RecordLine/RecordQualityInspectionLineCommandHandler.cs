namespace TradeSphere.Application.Features.QualityInspections.RecordLine;

public sealed class RecordQualityInspectionLineCommandHandler(
    IRepository<QualityInspection> inspectionRepository,
    IRepository<Product> productRepository,
    IApplicationDbContext dbContext) : IRequestHandler<RecordQualityInspectionLineCommand, Result>
{
    public async Task<Result> Handle(RecordQualityInspectionLineCommand request, CancellationToken cancellationToken)
    {
        var inspection = await inspectionRepository.GetByIdAsync(request.QualityInspectionId, cancellationToken)
            ?? throw new NotFoundException(nameof(QualityInspection), request.QualityInspectionId);

        if (!await productRepository.AnyAsync(new Domain.Specifications.ProductByIdSpecification(request.ProductId), cancellationToken))
            throw new NotFoundException(nameof(Product), request.ProductId);

        try
        {
            var line = inspection.RecordLine(request.ProductId, request.AcceptedQuantity, request.RejectedQuantity, request.MissingQuantity);
            await dbContext.Set<QualityInspectionLine>().AddAsync(line, cancellationToken);
        }
        catch (BusinessRuleViolationException ex)
        {
            return Result.Failure(ex.Message);
        }

        return Result.Success();
    }
}