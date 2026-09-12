namespace TradeSphere.Application.Features.Customers.GetList;

public sealed record GetCustomersQuery(string? SearchTerm, bool? IsActive, int PageNumber = 1, int PageSize = 20)
    : IRequest<PaginatedList<CustomerListItemDto>>;