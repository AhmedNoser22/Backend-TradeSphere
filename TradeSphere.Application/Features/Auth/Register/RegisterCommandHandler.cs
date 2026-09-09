namespace TradeSphere.Application.Features.Auth.Register;

public sealed class RegisterCommandHandler(
    IRepository<User> userRepository,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<RegisterCommand, Result<Guid>>
{
    private const UserRole DefaultRole = UserRole.SalesOfficer;
    public async Task<Result<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.AnyAsync(new UserByEmailSpecification(request.Email), cancellationToken))
            return Result<Guid>.Failure("A user with this email already exists.");

        var passwordHash = passwordHasher.Hash(request.Password);
        var confirmationCode = CodeGenerator.GenerateNumericCode();
        var expiresAtUtc = dateTimeProvider.UtcNow.AddMinutes(30);

        var user = new User(request.FullName, request.Email, passwordHash, DefaultRole, confirmationCode, expiresAtUtc);
        await userRepository.AddAsync(user, cancellationToken);

        return Result<Guid>.Success(user.Id);
    }
}