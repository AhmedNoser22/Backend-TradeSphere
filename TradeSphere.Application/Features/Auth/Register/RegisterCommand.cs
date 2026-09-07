namespace TradeSphere.Application.Features.Auth.Register;

public sealed record RegisterCommand(string FullName, string Email, string Password, UserRole Role)
    : IRequest<Result<Guid>>, ITransactionalRequest;