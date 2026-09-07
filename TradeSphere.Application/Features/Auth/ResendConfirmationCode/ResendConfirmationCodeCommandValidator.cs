namespace TradeSphere.Application.Features.Auth.ResendConfirmationCode;

public sealed class ResendConfirmationCodeCommandValidator : AbstractValidator<ResendConfirmationCodeCommand>
{
    public ResendConfirmationCodeCommandValidator() => RuleFor(x => x.Email).NotEmpty().EmailAddress();
}