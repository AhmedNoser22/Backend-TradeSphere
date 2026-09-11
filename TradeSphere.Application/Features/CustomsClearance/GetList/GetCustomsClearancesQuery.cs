namespace TradeSphere.Application.Features.CustomsClearance.GetList;
public sealed record GetCustomsClearancesQuery(
    CustomsClearanceStatus? Status, int PageNumber = 1, int PageSize = 20) : IRequest<PaginatedList<CustomsClearanceListItemDto>>;