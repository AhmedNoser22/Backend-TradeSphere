namespace TradeSphere.Application.Features.Users.CreateByAdmin;
public sealed class CreateUserByAdminCommandValidator : AbstractValidator<CreateUserByAdminCommand>
{
    public CreateUserByAdminCommandValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.");
        RuleFor(x => x.Role).IsInEnum();
    }
}