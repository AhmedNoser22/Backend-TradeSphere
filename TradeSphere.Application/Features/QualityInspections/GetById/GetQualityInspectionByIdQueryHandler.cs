namespace TradeSphere.Application.Features.QualityInspections.GetById;

public sealed class GetQualityInspectionByIdQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetQualityInspectionByIdQuery, QualityInspectionDetailsDto>
{
    public async Task<QualityInspectionDetailsDto> Handle(GetQualityInspectionByIdQuery request, CancellationToken cancellationToken)
    {
        var dto = await dbContext.QualityInspections
            .Where(qi => qi.Id == request.QualityInspectionId)
            .Select(qi => new QualityInspectionDetailsDto(
                qi.Id, qi.CustomsClearanceId, qi.PurchaseOrderId, qi.Status, qi.CompletedAtUtc,
                qi.Lines.Select(l => new QualityInspectionLineDto(l.Id, l.ProductId, l.Product.Name, l.AcceptedQuantity, l.RejectedQuantity, l.MissingQuantity)).ToList()))
            .FirstOrDefaultAsync(cancellationToken);

        return dto ?? throw new NotFoundException(nameof(QualityInspection), request.QualityInspectionId);
    }
}