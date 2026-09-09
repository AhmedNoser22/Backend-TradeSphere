namespace TradeSphere.Application.Features.Users.ActivateUser;

public sealed record ActivateUserCommand(Guid UserId) : IRequest<Result>, ITransactionalRequest;