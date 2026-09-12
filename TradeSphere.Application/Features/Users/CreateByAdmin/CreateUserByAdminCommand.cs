namespace TradeSphere.Application.Features.Users.CreateByAdmin;
public sealed record CreateUserByAdminCommand(string FullName, string Email, string Password, UserRole Role)
    : IRequest<Result<Guid>>, ITransactionalRequest;