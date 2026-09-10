namespace TradeSphere.Application.Features.Suppliers.GetSuppliers;
public sealed class GetSuppliersQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetSuppliersQuery, PaginatedList<SupplierDto>>
{
    public async Task<PaginatedList<SupplierDto>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Suppliers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(s => s.Name.Contains(request.SearchTerm) || s.Country.Contains(request.SearchTerm));

        if (request.IsActive.HasValue)
            query = query.Where(s => s.IsActive == request.IsActive.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(s => s.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectToType<SupplierDto>()
            .ToListAsync(cancellationToken);

        return new PaginatedList<SupplierDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}