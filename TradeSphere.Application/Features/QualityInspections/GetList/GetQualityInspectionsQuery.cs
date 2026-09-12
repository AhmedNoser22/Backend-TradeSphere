namespace TradeSphere.Application.Features.QualityInspections.GetList;

public sealed record GetQualityInspectionsQuery(
    QualityInspectionStatus? Status, int PageNumber = 1, int PageSize = 20) : IRequest<PaginatedList<QualityInspectionListItemDto>>;