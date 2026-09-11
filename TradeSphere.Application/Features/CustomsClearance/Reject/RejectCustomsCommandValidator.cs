namespace TradeSphere.Application.Features.CustomsClearance.Reject;

public sealed class RejectCustomsCommandValidator : AbstractValidator<RejectCustomsCommand>
{
    public RejectCustomsCommandValidator() => RuleFor(x => x.CustomsClearanceId).NotEmpty();
}