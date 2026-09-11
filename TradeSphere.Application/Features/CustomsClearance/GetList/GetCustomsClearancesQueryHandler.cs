namespace TradeSphere.Application.Features.CustomsClearance.GetList;

public sealed class GetCustomsClearancesQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetCustomsClearancesQuery, PaginatedList<CustomsClearanceListItemDto>>
{
    public async Task<PaginatedList<CustomsClearanceListItemDto>> Handle(GetCustomsClearancesQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.CustomsClearances
            .Join(dbContext.Shipments, c => c.ShipmentId, s => s.Id, (c, s) => new { Clearance = c, s.TrackingNumber })
            .AsQueryable();

        query = request.Status.HasValue
            ? query.Where(x => x.Clearance.Status == request.Status.Value)
            : query.Where(x => x.Clearance.Status == Domain.Enums.CustomsClearanceStatus.PendingDocuments
                             || x.Clearance.Status == Domain.Enums.CustomsClearanceStatus.UnderClearance);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.Clearance.CreatedAtUtc)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new CustomsClearanceListItemDto(
                x.Clearance.Id, x.Clearance.ShipmentId, x.TrackingNumber, x.Clearance.DeclarationNumber, x.Clearance.Port,
                x.Clearance.DeclaredGoodsValue, x.Clearance.CustomsDuties, x.Clearance.ClearanceFees,
                x.Clearance.TotalLandedCost, x.Clearance.Status))
            .ToListAsync(cancellationToken);

        return new PaginatedList<CustomsClearanceListItemDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}