namespace TradeSphere.Application.Features.Users.ChangeUserRole;

public sealed record ChangeUserRoleCommand(Guid UserId, UserRole NewRole) : IRequest<Result>, ITransactionalRequest;