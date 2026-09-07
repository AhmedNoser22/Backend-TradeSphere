namespace TradeSphere.Application.Features.Auth.Register;

public sealed class RegisterCommandHandler(
    IRepository<User> userRepository,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<RegisterCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existing = await userRepository.ListAsync(new UserByEmailSpecification(request.Email), cancellationToken);
        if (existing.Count != 0)
            return Result<Guid>.Failure("A user with this email already exists.");

        var passwordHash = passwordHasher.Hash(request.Password);
        var confirmationCode = CodeGenerator.GenerateNumericCode();
        var expiresAtUtc = dateTimeProvider.UtcNow.AddMinutes(30);

        var user = new User(request.FullName, request.Email, passwordHash, request.Role, confirmationCode, expiresAtUtc);
        await userRepository.AddAsync(user, cancellationToken);

        return Result<Guid>.Success(user.Id);
    }
}