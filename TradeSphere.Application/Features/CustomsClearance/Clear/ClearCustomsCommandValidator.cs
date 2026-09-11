namespace TradeSphere.Application.Features.CustomsClearance.Clear;

public sealed class ClearCustomsCommandValidator : AbstractValidator<ClearCustomsCommand>
{
    public ClearCustomsCommandValidator()
    {
        RuleFor(x => x.CustomsClearanceId).NotEmpty();
        RuleFor(x => x.CustomsDuties).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ClearanceFees).GreaterThanOrEqualTo(0);
    }
}