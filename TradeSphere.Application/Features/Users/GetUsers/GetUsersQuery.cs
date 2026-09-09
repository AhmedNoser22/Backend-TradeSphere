namespace TradeSphere.Application.Features.Users.GetUsers;

public sealed record GetUsersQuery(
    string? SearchTerm,
    UserRole? Role,
    bool? IsActive,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<PaginatedList<UserListItemDto>>;