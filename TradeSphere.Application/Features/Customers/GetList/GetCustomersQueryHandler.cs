namespace TradeSphere.Application.Features.Customers.GetList;

public sealed class GetCustomersQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetCustomersQuery, PaginatedList<CustomerListItemDto>>
{
    public async Task<PaginatedList<CustomerListItemDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Customers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(c => c.Name.Contains(request.SearchTerm));

        if (request.IsActive.HasValue)
            query = query.Where(c => c.IsActive == request.IsActive.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CustomerListItemDto(c.Id, c.Name, c.Contact.Email, c.Contact.Phone, c.IsActive))
            .ToListAsync(cancellationToken);

        return new PaginatedList<CustomerListItemDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}