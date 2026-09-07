namespace TradeSphere.Application.Features.Auth.ConfirmEmail;

public sealed record ConfirmEmailCommand(Guid UserId, string Code) : IRequest<Result>, ITransactionalRequest;