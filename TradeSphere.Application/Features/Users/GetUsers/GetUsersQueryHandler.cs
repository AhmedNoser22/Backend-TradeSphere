namespace TradeSphere.Application.Features.Users.GetUsers;

public sealed class GetUsersQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetUsersQuery, PaginatedList<UserListItemDto>>
{
    public async Task<PaginatedList<UserListItemDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(u => u.FullName.Contains(request.SearchTerm) || u.Email.Contains(request.SearchTerm));

        if (request.Role.HasValue)
            query = query.Where(u => u.Role == request.Role.Value);

        if (request.IsActive.HasValue)
            query = query.Where(u => u.IsActive == request.IsActive.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        // ProjectToType (Mapster) builds the SQL SELECT with only the DTO's
        // columns — not the full User row — which matters for a list endpoint.
        var items = await query
            .OrderByDescending(u => u.CreatedAtUtc)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectToType<UserListItemDto>()
            .ToListAsync(cancellationToken);

        return new PaginatedList<UserListItemDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}