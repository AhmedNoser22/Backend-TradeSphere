namespace TradeSphere.Application.Features.Auth.ResendConfirmationCode;

public sealed record ResendConfirmationCodeCommand(string Email) : IRequest<Result>, ITransactionalRequest;