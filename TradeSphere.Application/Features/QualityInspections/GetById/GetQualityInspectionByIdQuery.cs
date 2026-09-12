namespace TradeSphere.Application.Features.QualityInspections.GetById;

public sealed record GetQualityInspectionByIdQuery(Guid QualityInspectionId) : IRequest<QualityInspectionDetailsDto>;