namespace TradeSphere.Application.Features.QualityInspections.GetList;

public sealed class GetQualityInspectionsQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetQualityInspectionsQuery, PaginatedList<QualityInspectionListItemDto>>
{
    public async Task<PaginatedList<QualityInspectionListItemDto>> Handle(GetQualityInspectionsQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.QualityInspections.AsQueryable();

        if (request.Status.HasValue)
            query = query.Where(qi => qi.Status == request.Status.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(qi => qi.CreatedAtUtc)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(qi => new QualityInspectionListItemDto(qi.Id, qi.CustomsClearanceId, qi.PurchaseOrderId, qi.Status, qi.CompletedAtUtc))
            .ToListAsync(cancellationToken);

        return new PaginatedList<QualityInspectionListItemDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}