namespace TradeSphere.Application.Features.Users.ChangeUserRole;

public sealed class ChangeUserRoleCommandValidator : AbstractValidator<ChangeUserRoleCommand>
{
    public ChangeUserRoleCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.NewRole).IsInEnum();
    }
}