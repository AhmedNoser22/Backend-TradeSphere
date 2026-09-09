namespace TradeSphere.Application.Features.Users.DeactivateUser;

public sealed record DeactivateUserCommand(Guid UserId) : IRequest<Result>, ITransactionalRequest;