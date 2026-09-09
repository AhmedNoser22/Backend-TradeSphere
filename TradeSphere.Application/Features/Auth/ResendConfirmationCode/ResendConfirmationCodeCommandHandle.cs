namespace TradeSphere.Application.Features.Auth.ResendConfirmationCode;

public sealed class ResendConfirmationCodeCommandHandler(
    IRepository<User> userRepository,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<ResendConfirmationCodeCommand, Result>
{
    public async Task<Result> Handle(ResendConfirmationCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(new UserByEmailSpecification(request.Email), cancellationToken);

        if (user is null || user.EmailConfirmed)
            return Result.Success();

        var code = CodeGenerator.GenerateNumericCode();
        var expiresAtUtc = dateTimeProvider.UtcNow.AddMinutes(30);
        user.ResendEmailConfirmationCode(code, expiresAtUtc);

        userRepository.Update(user);
        return Result.Success();
    }
}