namespace TradeSphere.Application.Features.Users.CreateByAdmin;

public sealed class CreateUserByAdminCommandHandler(
    IRepository<User> userRepository,
    IPasswordHasher passwordHasher) : IRequestHandler<CreateUserByAdminCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateUserByAdminCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.AnyAsync(new UserByEmailSpecification(request.Email), cancellationToken))
            return Result<Guid>.Failure("A user with this email already exists.");

        var passwordHash = passwordHasher.Hash(request.Password);
        var user = User.CreateByAdmin(request.FullName, request.Email, passwordHash, request.Role);

        await userRepository.AddAsync(user, cancellationToken);
        return Result<Guid>.Success(user.Id);
    }
}