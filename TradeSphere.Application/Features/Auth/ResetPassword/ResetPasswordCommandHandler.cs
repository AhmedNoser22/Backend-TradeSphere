namespace TradeSphere.Application.Features.Auth.ResetPassword;

public sealed class ResetPasswordCommandHandler(
    IRepository<User> userRepository,
    IPasswordHasher passwordHasher) : IRequestHandler<ResetPasswordCommand, Result>
{
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(new UserByEmailSpecification(request.Email), cancellationToken);
        if (user is null)
            return Result.Failure("Invalid request.");

        try
        {
            user.ResetPassword(request.Code, passwordHasher.Hash(request.NewPassword));
        }
        catch (BusinessRuleViolationException ex)
        {
            return Result.Failure(ex.Message);
        }

        userRepository.Update(user);
        return Result.Success();
    }
}