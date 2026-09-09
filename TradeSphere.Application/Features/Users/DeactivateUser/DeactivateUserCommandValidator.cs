namespace TradeSphere.Application.Features.Users.DeactivateUser;

public sealed class DeactivateUserCommandValidator : AbstractValidator<DeactivateUserCommand>
{
    public DeactivateUserCommandValidator() => RuleFor(x => x.UserId).NotEmpty();
}