namespace TradeSphere.Application.Features.Users.Common;
public sealed record UserListItemDto(
    Guid Id,
    string FullName,
    string Email,
    UserRole Role,
    bool IsActive,
    bool EmailConfirmed,
    DateTimeOffset CreatedAtUtc);