namespace TradeSphere.Application.Features.Auth.ConfirmEmail;

public sealed class ConfirmEmailCommandHandler(IRepository<User> userRepository) : IRequestHandler<ConfirmEmailCommand, Result>
{
    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure("User not found.");

        try
        {
            user.ConfirmEmail(request.Code);
        }
        catch (BusinessRuleViolationException ex)
        {
            return Result.Failure(ex.Message);
        }

        userRepository.Update(user);
        return Result.Success();
    }
}