namespace TradeSphere.Application.Features.Auth.ForgotPassword;

public sealed class ForgotPasswordCommandHandler(
    IRepository<User> userRepository,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<ForgotPasswordCommand, Result>
{
    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(new UserByEmailSpecification(request.Email), cancellationToken);

        if (user is null)
            return Result.Success();

        var code = CodeGenerator.GenerateNumericCode();
        var expiresAtUtc = dateTimeProvider.UtcNow.AddMinutes(15);
        user.RequestPasswordReset(code, expiresAtUtc);

        userRepository.Update(user);
        return Result.Success();
    }
}