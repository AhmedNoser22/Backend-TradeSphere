namespace TradeSphere.Application.Features.Auth.ResetPassword;

public sealed record ResetPasswordCommand(string Email, string Code, string NewPassword) : IRequest<Result>, ITransactionalRequest;